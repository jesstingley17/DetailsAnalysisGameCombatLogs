using Chat.Application.DTOs;

namespace Chat.Application.Interfaces;

public interface IVoiceChatService
{
    Task<VoiceChatDto> CreateAsync(VoiceChatDto item);

    Task DeleteAsync(string id);

    Task<IEnumerable<VoiceChatDto>> GetAllAsync();

    Task<VoiceChatDto> GetByIdAsync(string id);
}
