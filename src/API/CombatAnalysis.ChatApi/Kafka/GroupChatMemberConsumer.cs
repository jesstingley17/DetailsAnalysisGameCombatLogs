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

public class GroupChatMemberConsumer(IOptions<KafkaSettings> kafkaSettings, ILogger<GroupChatMemberConsumer> logger, IServiceScopeFactory serviceScopeFactory,
    IMapper mapper, IKafkaProducerService<string, string> kafkaProducer) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChatMember, logger)
{
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
            _logger.LogError(ex, "Consume Chat API data failed: Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, ConsumeResult<string, JsonDocument> kafkaData)
    {
        try
        {
            var action = kafkaData.Message.Value.Deserialize<GroupChatMemberAction>();
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            ArgumentNullException.ThrowIfNull(action.User, nameof(action.User));

            switch (action.State)
            {
                case (int)ChatMembersActionState.AddUser:
                    var chatUser = await CreateGroupChatUser(scope, action.User);

                    await CreateSystemMessageAsync( $"Add user '{action.User.Username}' to chat", action);

                    break;
                case (int)ChatMembersActionState.RemoveUser:
                    await CreateSystemMessageAsync($"Remove user '{action.User.Username}' from chat", action);

                    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Create group chat user failed:  Parameter '{ParamName}' was null.", ex.ParamName);
        }
    }

    private async Task<GroupChatUserModel> CreateGroupChatUser(IServiceScope scope, GroupChatUserModel chatUser)
    {
        var chatUserService = scope.ServiceProvider.GetService<IServiceTransaction<GroupChatUserDto, string>>();
        ArgumentNullException.ThrowIfNull(chatUserService, nameof(chatUserService));

        chatUser.Id = Guid.NewGuid().ToString();

        var map = _mapper.Map<GroupChatUserDto>(chatUser);
        var createdGroupChatUser = await chatUserService.CreateAsync(map);
        ArgumentNullException.ThrowIfNull(createdGroupChatUser, nameof(createdGroupChatUser));

        return chatUser;
    }

    private async Task CreateSystemMessageAsync(string systemMessage, GroupChatMemberAction chatAction)
    {
        var chatMessageAction = JsonSerializer.Serialize(new GroupChatMessageAction
        {
            Message = new GroupChatMessageModel
            {
                ChatId = chatAction.User.ChatId,
                GroupChatUserId = chatAction.User.Id,
                Message = systemMessage,
                Status = 2,
                Time = chatAction.When,
                Type = (int)MessageType.System,
                Username = "System"
            },
            State = (int)ChatMessageActionState.Created,
            When = DateTime.UtcNow.ToString(),
            RefreshToken = chatAction.RefreshToken,
            AccessToken = chatAction.AccessToken
        });
        await _kafkaProducer.ProduceAsync(KafkaTopics.GroupChatMessage, Guid.NewGuid().ToString(), chatMessageAction);
    }
}
