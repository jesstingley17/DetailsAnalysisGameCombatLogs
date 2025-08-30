using AutoMapper;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class GroupChatConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, 
    ILogger<GroupChatConsumer> logger, IServiceScopeFactory serviceScopeFactory, IMapper mapper) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChat, logger)
{
    private readonly IOptions<Hubs> _hubs = hubs;
    private readonly ILogger<GroupChatConsumer> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IMapper _mapper = mapper;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

            using var scope = _serviceScopeFactory.CreateScope();
            var chatTransaction = scope.ServiceProvider.GetService<IChatTransactionService>();
            ArgumentNullException.ThrowIfNull(chatTransaction, nameof(chatTransaction));

            await ExecuteAsync(scope, chatTransaction, kafkaData);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Chat API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, IChatTransactionService chatTransaction, ConsumeResult<string, JsonDocument> kafkaData)
    {
        try
        {
            var action = kafkaData.Message.Value.Deserialize<GroupChatAction>();
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            ArgumentNullException.ThrowIfNull(action.Rules, nameof(action.Rules));
            ArgumentNullException.ThrowIfNull(action.User, nameof(action.User));

            switch(action.State)
            {
                case (int)ChatActionState.Created:
                    await chatTransaction.BeginTransactionAsync();

                    var chatId = await CreateChatRefsAsync(scope, action.Chat, action.Rules, action.User);

                    await chatTransaction.CommitTransactionAsync();

                    await SendSignalRequestChatsAsync(scope, action, chatId);

                    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat refs failed: Parameter '{ParamName}' was null.", ex.ParamName);

            await chatTransaction.RollbackTransactionAsync();
        }
        catch (Exception)
        {
            await chatTransaction.RollbackTransactionAsync();

            throw;
        }
    }

    private async Task<int> CreateChatRefsAsync(IServiceScope scope, GroupChatModel chat, GroupChatRulesModel chatRules, GroupChatUserModel chatUser)
    {
        var chatService = scope.ServiceProvider.GetService<IService<GroupChatDto, int>>();
        ArgumentNullException.ThrowIfNull(chatService, nameof(chatService));

        var chatRulesService = scope.ServiceProvider.GetService<IService<GroupChatRulesDto, int>>();
        ArgumentNullException.ThrowIfNull(chatRulesService, nameof(chatRulesService));

        var chatUserService = scope.ServiceProvider.GetService<IServiceTransaction<GroupChatUserDto, string>>();
        ArgumentNullException.ThrowIfNull(chatUserService, nameof(chatUserService));

        var map = _mapper.Map<GroupChatDto>(chat);
        var createdChat = await chatService.CreateAsync(map);
        ArgumentNullException.ThrowIfNull(createdChat, nameof(createdChat));

        chatRules.ChatId = createdChat.Id;

        var chatRulesMap = _mapper.Map<GroupChatRulesDto>(chatRules);
        var createdChatRules = await chatRulesService.CreateAsync(chatRulesMap);
        ArgumentNullException.ThrowIfNull(createdChatRules, nameof(createdChatRules));

        chatUser.ChatId = createdChat.Id;

        var chatUserMap = _mapper.Map<GroupChatUserDto>(chatUser);
        var createdChatUser = await chatUserService.CreateAsync(chatUserMap);
        ArgumentNullException.ThrowIfNull(createdChatUser, nameof(createdChatUser));

        return createdChat.Id;
    }

    private async Task SendSignalRequestChatsAsync(IServiceScope scope, GroupChatAction chatAction, int chatId)
    {
        var chatHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
        ArgumentNullException.ThrowIfNull(chatHubHelper, nameof(chatHubHelper));

        await chatHubHelper.ConnectToHubAsync($"{_hubs.Value.Server}{_hubs.Value.GroupChatAddress}", chatAction.RefreshToken, chatAction.AccessToken);
        await chatHubHelper.JoinRoomAsync(chatId);
        await chatHubHelper.RequestsChats(chatId, chatAction.User.AppUserId);
    }
}
