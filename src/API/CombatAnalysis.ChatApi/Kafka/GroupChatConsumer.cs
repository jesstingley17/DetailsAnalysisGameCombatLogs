using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Infrastructure.Persistence;
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

public class GroupChatConsumer(IOptions<KafkaSettings> kafkaSettings, IOptions<Hubs> hubs, 
    ILogger<GroupChatConsumer> logger, IServiceScopeFactory serviceScopeFactory, IMapper mapper) : KafkaConsumerBase(kafkaSettings, KafkaTopics.GroupChat, logger)
{
    private readonly Hubs _hubs = hubs.Value;
    private readonly ILogger<GroupChatConsumer> _logger = logger;
    private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
    private readonly IMapper _mapper = mapper;

    protected override async Task ConsumeMessageAsync(ConsumeResult<string, JsonDocument> kafkaData, CancellationToken stoppingToken)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(kafkaData, nameof(kafkaData));

            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ChatContext>();
            ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));

            await ExecuteAsync(scope, dbContext, kafkaData);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "Consume Group Chat data (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChat, ex.ParamName);
        }
    }

    private async Task ExecuteAsync(IServiceScope scope, ChatContext dbContext, ConsumeResult<string, JsonDocument> kafkaData)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var action = kafkaData.Message.Value.Deserialize<GroupChatAction>();
            ArgumentNullException.ThrowIfNull(action, nameof(action));
            ArgumentNullException.ThrowIfNull(action.Rules, nameof(action.Rules));
            ArgumentNullException.ThrowIfNull(action.User, nameof(action.User));

            switch(action.State)
            {
                case ChatActionState.Created:
                    var chatId = await CreateChatRefsAsync(scope, action.Chat, action.Rules, action.User);

                    await transaction.CommitAsync();

                    await SendSignalRequestChatsAsync(scope, action, chatId);

                    break;
                case ChatActionState.Removed:
                    await RemoveChatRefsAsync(scope, action.Chat.Id);

                    await transaction.CommitAsync();

                    break;
            }
        }
        catch (ArgumentNullException ex)
        {
            await transaction.RollbackAsync();

            _logger.LogError(ex, "Create group chat from Kafka Consumer (topic: {Topic}) failed. Parameter '{ParamName}' was null.", KafkaTopics.GroupChat, ex.ParamName);
        }
        catch (DbUpdateException ex)
        {
            await transaction.RollbackAsync();

            _logger.LogError(ex, "Create group chat from Kafka Consumer (topic: {Topic}) failed.", KafkaTopics.GroupChat);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();

            throw;
        }
    }

    private async Task<int> CreateChatRefsAsync(IServiceScope scope, GroupChatModel chat, GroupChatRulesModel chatRules, GroupChatUserModel chatUser)
    {
        var chatService = scope.ServiceProvider.GetService<IGroupChatService>();
        ArgumentNullException.ThrowIfNull(chatService, nameof(chatService));

        var chatUserService = scope.ServiceProvider.GetService<IGroupChatUserService>();
        ArgumentNullException.ThrowIfNull(chatUserService, nameof(chatUserService));

        var chatMap = _mapper.Map<GroupChatDto>(chat);
        var createdChat = await chatService.CreateAsync(chatMap);

        var updatedChat = chatRules with { GroupChatId = createdChat.Id };

        var chatRulesMap = _mapper.Map<GroupChatRulesDto>(updatedChat);
        await chatService.AddRulesAsync(chatRulesMap);

        var updatedUser = chatUser with { GroupChatId = createdChat.Id };

        var chatUserMap = _mapper.Map<GroupChatUserDto>(updatedUser);
        await chatUserService.CreateAsync(chatUserMap);

        return createdChat.Id;
    }

    private static async Task RemoveChatRefsAsync(IServiceScope scope, int chatId)
    {
        var chatService = scope.ServiceProvider.GetService<IGroupChatService>();
        ArgumentNullException.ThrowIfNull(chatService, nameof(chatService));

        await chatService.DeleteAsync(chatId);
    }

    private async Task SendSignalRequestChatsAsync(IServiceScope scope, GroupChatAction chatAction, int chatId)
    {
        var chatHubHelper = scope.ServiceProvider.GetService<IChatHubHelper>();
        ArgumentNullException.ThrowIfNull(chatHubHelper, nameof(chatHubHelper));

        await chatHubHelper.ConnectToHubAsync($"{_hubs.Server}{_hubs.GroupChatAddress}", chatAction.RefreshToken, chatAction.AccessToken);
        await chatHubHelper.JoinRoomAsync(chatId);
        await chatHubHelper.RequestsChats(chatId, chatAction.User.AppUserId);
    }
}
