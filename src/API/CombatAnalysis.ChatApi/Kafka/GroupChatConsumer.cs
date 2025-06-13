using AutoMapper;
using CombatAnalysis.ChatApi.Consts;
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

            await CreateChatRefsAsync(scope, chatAction.ChatId, chatAction.Rules, chatAction.User);

            await chatTransaction.CommitTransactionAsync();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat refs failed: Parameter '{ParamName}' was null.", ex.ParamName);

            await chatTransaction.RollbackTransactionAsync();
        }
        catch (ArgumentOutOfRangeException ex)
        {
            _logger.LogError(ex, "Invalid argument: Parameter '{ParamName}' was out of range.", ex.ParamName);

            await chatTransaction.RollbackTransactionAsync();
        }
        catch (Exception)
        {
            await chatTransaction.RollbackTransactionAsync();

            throw;
        }
    }

    private async Task CreateChatRefsAsync(IServiceScope scope, int chatId, GroupChatRulesModel chatRules, GroupChatUserModel chatUser)
    {
        ArgumentOutOfRangeException.ThrowIfZero(chatId, nameof(chatId));

        var chatRulesService = scope.ServiceProvider.GetService<IService<GroupChatRulesDto, int>>();
        ArgumentNullException.ThrowIfNull(chatRulesService, nameof(chatRulesService));

        var chatUserService = scope.ServiceProvider.GetService<IServiceTransaction<GroupChatUserDto, string>>();
        ArgumentNullException.ThrowIfNull(chatUserService, nameof(chatUserService));

        chatRules.ChatId = chatId;

        var chatRulesMap = _mapper.Map<GroupChatRulesDto>(chatRules);
        var createdChatRules = await chatRulesService.CreateAsync(chatRulesMap);
        ArgumentNullException.ThrowIfNull(createdChatRules, nameof(createdChatRules));

        chatUser.ChatId = chatId;

        var chatUserMap = _mapper.Map<GroupChatUserDto>(chatUser);
        var createdChatUser = await chatUserService.CreateAsync(chatUserMap);
        ArgumentNullException.ThrowIfNull(createdChatUser, nameof(createdChatUser));
    }
}
