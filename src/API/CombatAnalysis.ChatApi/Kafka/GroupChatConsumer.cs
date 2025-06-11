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
            ArgumentNullException.ThrowIfNull(kafkaData);

            using var scope = _serviceScopeFactory.CreateScope();
            var chatTransaction = scope.ServiceProvider.GetService<IChatTransactionService>();
            ArgumentNullException.ThrowIfNull(chatTransaction);

            await ExecuteAsync(scope, chatTransaction, kafkaData);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Some argument receavied as null while consume Chat API data: ${ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while consume Chat API data: {ex.Message}");
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, IChatTransactionService chatTransaction, ConsumeResult<string, JsonDocument> kafkaData)
    {
        try
        {
            await chatTransaction.BeginTransactionAsync();

            var chatAction = kafkaData.Message.Value.Deserialize<GroupChatAction>();
            ArgumentNullException.ThrowIfNull(chatAction);

            await CreateChatRefsAsync(scope, chatAction.ChatId, chatAction.Rules, chatAction.User);

            await chatTransaction.CommitTransactionAsync();
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, $"Some argument receavied as null while consume Chat API data: ${ex.Message}");

            await chatTransaction.RollbackTransactionAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while consume Chat API data: {ex.Message}");

            await chatTransaction.RollbackTransactionAsync();
        }
    }

    private async Task CreateChatRefsAsync(IServiceScope scope, int chatId, GroupChatRulesModel chatRules, GroupChatUserModel chatUser)
    {
        var chatRulesService = scope.ServiceProvider.GetService<IService<GroupChatRulesDto, int>>();
        ArgumentNullException.ThrowIfNull(chatRulesService);

        var chatUserService = scope.ServiceProvider.GetService<IServiceTransaction<GroupChatUserDto, string>>();
        ArgumentNullException.ThrowIfNull(chatUserService);

        var chatMessageCountService = scope.ServiceProvider.GetService<IService<GroupChatMessageCountDto, int>>();
        ArgumentNullException.ThrowIfNull(chatMessageCountService);

        chatRules.ChatId = chatId;

        var chatRulesMap = _mapper.Map<GroupChatRulesDto>(chatRules);
        await chatRulesService.CreateAsync(chatRulesMap);

        chatUser.ChatId = chatId;

        var chatUserMap = _mapper.Map<GroupChatUserDto>(chatUser);
        var result = await chatUserService.CreateAsync(chatUserMap);

        var messageCount = new GroupChatMessageCountDto
        {
            ChatId = chatId,
            GroupChatUserId = result.Id,
            Count = 0
        };

        await chatMessageCountService.CreateAsync(messageCount);
    }
}
