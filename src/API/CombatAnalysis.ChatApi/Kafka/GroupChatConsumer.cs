using AutoMapper;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatApi.Models;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class GroupChatConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<GroupChatConsumer> logger, IServiceScopeFactory serviceScopeFactory,
    IMapper mapper) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChat, logger)
{
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
            await chatTransaction.BeginTransactionAsync();

            var chatAction = kafkaData.Message.Value.Deserialize<GroupChatAction>();
            ArgumentNullException.ThrowIfNull(chatAction, nameof(chatAction));
            ArgumentNullException.ThrowIfNull(chatAction.Rules, nameof(chatAction.Rules));
            ArgumentNullException.ThrowIfNull(chatAction.User, nameof(chatAction.User));

            var chatId = await CreateChatRefsAsync(scope, chatAction.Chat, chatAction.Rules, chatAction.User);

            await chatTransaction.CommitTransactionAsync();

            await SendSignalAsync(scope, chatAction, chatId);
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

    private static async Task SendSignalAsync(IServiceScope scope, GroupChatAction chatAction, int chatId)
    {
        var chatHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
        ArgumentNullException.ThrowIfNull(chatHubHelper, nameof(chatHubHelper));

        await chatHubHelper.ConnectToUnreadMessageHubAsync("https://localhost:7026/groupChatHub", chatAction.RefreshToken, chatAction.AccessToken);
        await chatHubHelper.JoinRoomAsync(chatId);
        await chatHubHelper.RequestsChats(chatId, chatAction.User.AppUserId);
    }
}
