using CombatAnalysis.Core.Models.Chat;

namespace CombatAnalysis.Core.Interfaces.Services;

public interface IPersonalChatService
{
    Task<IEnumerable<PersonalChatMessageModel>> LoadMessagesAsync(int chatId);

    Task CreateNewPersonalChatAsync(string accountId, string companionId);

    Task<IEnumerable<PersonalChatModel>> LoadPersonalChatsAsync(string accountId);

    Task UpdatePersonalChatAsync(PersonalChatModel chat, string accountId);
}
