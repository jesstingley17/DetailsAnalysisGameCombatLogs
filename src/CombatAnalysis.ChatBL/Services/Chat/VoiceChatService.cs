using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.ChatBL.Services.Chat;

internal class VoiceChatService(IGenericRepository<VoiceChat, string> repository, IMapper mapper) : IService<VoiceChatDto, string>
{
    private readonly IGenericRepository<VoiceChat, string> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<VoiceChatDto?> CreateAsync(VoiceChatDto item)
    {
        var map = _mapper.Map<VoiceChat>(item);
        var createdItem = await _repository.CreateAsync(map);
        if (createdItem == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<VoiceChatDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<VoiceChatDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<VoiceChatDto>>(allData);

        return result;
    }

    public async Task<VoiceChatDto?> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        if (result == null)
        {
            return null;
        }

        var resultMap = _mapper.Map<VoiceChatDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<VoiceChatDto>> GetByParamAsync<TValue>(Expression<Func<VoiceChatDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<VoiceChat, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<VoiceChatDto>>(result);

        return resultMap;
    }

    public async Task UpdateAsync(VoiceChatDto item)
    {
        var map = _mapper.Map<VoiceChat>(item);
        await _repository.UpdateAsync(map);
    }
}
