using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Domain.Enums;
using Chat.Infrastructure.Exceptions;
using CombatAnalysis.ChatApi.Consts;
using CombatAnalysis.ChatApi.Enums;
using CombatAnalysis.ChatApi.Interfaces;
using CombatAnalysis.ChatApi.Kafka.Actions;
using CombatAnalysis.ChatApi.Models;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace CombatAnalysis.ChatApi.Kafka;

public class GroupChatMemberConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, ILogger<GroupChatMemberConsumer> logger, IServiceScopeFactory serviceScopeFactory,
    IMapper mapper, IKafkaProducerService<string, string> kafkaProducer) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChatMember, logger)
{
    private readonly Hubs _hubs = hubs.Value;
    private readonly ILogger<GroupChatMemberConsumer> _logger = logger;
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
            _logger.LogError(ex, "Consume Group Chat Member data (topic: {Topic}) failed: Parameter '{ParamName}' was null.", KafkaTopics.GroupChatMember, ex.ParamName);
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, ConsumeResult<string, JsonDocument> kafkaData)
    {
        try
        {
            var action = kafkaData.Message.Value.Deserialize<GroupChatMemberAction>();
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            ArgumentNullException.ThrowIfNull(action.User, nameof(action.User));

            var chatUserService = scope.ServiceProvider.GetService<IGroupChatUserService>();
            ArgumentNullException.ThrowIfNull(chatUserService, nameof(chatUserService));

            var chatOwnerUser = await chatUserService.FindByAppUserIdAsync(action.User.GroupChatId, action.ChatOwnerId);

            switch (action.State)
            {
                case ChatMembersActionState.AddUser:
                    await CreateGroupChatUser(chatUserService, action.User);

                    await SendSignalRequestChatsAsync(scope, action);
                    await CreateSystemMessageAsync( $"Add user '{action.User.Username}' to chat", action, chatOwnerUser.Id);

                    break;
                case ChatMembersActionState.RemoveUser:
                    await RemoveGroupChatUser(chatUserService, action.User.Id);

                    await CreateSystemMessageAsync($"Remove user '{action.User.Username}' from chat", action, chatOwnerUser.Id);

                    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat user from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChatMember, ex.ParamName);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning("Delete group chat user from Kafka Consumer (topic: {Topic}) failed. Group chat user {Id} not found.", KafkaTopics.GroupChatMember, ex.EntityId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to create chat user from Kafka Consumer (topic: {Topic})", KafkaTopics.GroupChatMember);
        }
    }

    private async Task<GroupChatUserModel> CreateGroupChatUser(IGroupChatUserService chatUserService, GroupChatUserModel chatUser)
    {
        var map = _mapper.Map<GroupChatUserDto>(chatUser);
        var createdGroupChatUser = await chatUserService.CreateAsync(map);

        var mapToModel = _mapper.Map<GroupChatUserModel>(createdGroupChatUser);
        return mapToModel;
    }

    private async Task SendSignalRequestChatsAsync(IServiceScope scope, GroupChatMemberAction chatAction)
    {
        var chatHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
        ArgumentNullException.ThrowIfNull(chatHubHelper, nameof(chatHubHelper));

        await chatHubHelper.ConnectToHubAsync($"{_hubs.Server}{_hubs.GroupChatAddress}", chatAction.RefreshToken, chatAction.AccessToken);
        await chatHubHelper.JoinRoomAsync(chatAction.User.GroupChatId);
        await chatHubHelper.RequestsChatsAsync(chatAction.User.GroupChatId, chatAction.User.AppUserId);
    }

    private async Task CreateSystemMessageAsync(string systemMessage, GroupChatMemberAction chatAction, string chatOwnerUserId)
    {
        var chatMessageAction = JsonSerializer.Serialize(new GroupChatMessageAction
        {
            ChatMessage = new GroupChatMessageModel(0, "System", systemMessage, chatAction.When, MessageStatus.Sent, MessageType.System, MessageMarkedType.None, false, chatAction.User.GroupChatId, chatOwnerUserId),
            State = (int)ChatMessageActionState.Created,
            When = DateTimeOffset.UtcNow,
            RefreshToken = chatAction.RefreshToken,
            AccessToken = chatAction.AccessToken
        });
        await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMessage, Guid.NewGuid().ToString(), chatMessageAction);
    }

    private static async Task RemoveGroupChatUser(IGroupChatUserService chatUserService, string chatUserId)
    {
        await chatUserService.DeleteAsync(chatUserId);
    }
}
