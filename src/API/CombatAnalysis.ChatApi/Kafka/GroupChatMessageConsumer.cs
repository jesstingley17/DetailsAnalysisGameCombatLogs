using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Enums;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatApi.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class GroupChatMessageConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<GroupChatMessageConsumer> logger, 
    IServiceScopeFactory serviceScopeFactory, IMapper mapper, IKafkaProducerService<string, string> kafkaProducer) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChatMessage, logger)
{
    private readonly IOptions<Hubs> _hubs = hubs;
    private readonly ILogger<GroupChatMessageConsumer> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IMapper _mapper = mapper;
    private readonly IKafkaProducerService<string, string> _kafkaProducer = kafkaProducer;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

            using var scope = _serviceScopeFactory.CreateScope();

            await ExecuteAsync(scope, kafkaData);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Chat API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, ConsumeResult<string, JsonDocument> kafkaData)
    {
        try
        {
            var chatMessgaeService = scope.ServiceProvider.GetService<IGroupChatMessageService>();
            ArgumentNullException.ThrowIfNull(chatMessgaeService, nameof(chatMessgaeService));

            var chatHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
            ArgumentNullException.ThrowIfNull(chatHubHelper, nameof(chatHubHelper));

            var action = kafkaData.Message.Value.Deserialize<GroupChatMessageAction>();
            ArgumentNullException.ThrowIfNull(action, nameof(action));

            var map = _mapper.Map<GroupChatMessageDto>(action.Message);
            var createdMessage = await chatMessgaeService.CreateAsync(map);
            ArgumentNullException.ThrowIfNull(createdMessage, nameof(createdMessage));

            var mapToModel = _mapper.Map<GroupChatMessageModel>(createdMessage);
            action.Message = mapToModel;

            await chatHubHelper.ConnectToHubAsync($"{_hubs.Value.Server}{_hubs.Value.GroupChatMessagesAddress}", action.RefreshToken, action.AccessToken);
            await chatHubHelper.JoinRoomAsync(createdMessage.GroupChatId);

            await chatHubHelper.RequestsMessage(createdMessage.GroupChatId, action.Message);

            if (createdMessage.Type != MessageType.System)
            {
                await IncreaseUnreadMessageRequestAsync(createdMessage.Id, action);
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Chat API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task IncreaseUnreadMessageRequestAsync(int messageId, GroupChatMessageAction action)
    {
        var unreadMessageAction = JsonSerializer.Serialize(new GroupChatUnreadMessageAction
        {
            ChatId = action.Message.GroupChatId,
            MessageId = messageId,
            GroupChatUserId = action.Message.GroupChatUserId,
            State = (int)ChatMessageActionState.Created,
            RefreshToken = action.RefreshToken,
            AccessToken = action.AccessToken,
        });
        await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatUnreadMessage, Guid.NewGuid().ToString(), unreadMessageAction);
    }
}
