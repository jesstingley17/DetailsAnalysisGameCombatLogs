using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Chat.Application.DTOs;
using Chat.Domain.Aggregates;
using System.Linq.Expressions;

namespace Chat.Application.Mappers;

public static class VoiceChatMapper
{
    public static VoiceChat ToEntity(this VoiceChatDto voiceChatDto, IMapper mapper)
    {
        var map = mapper.Map<VoiceChat>(voiceChatDto);

        return map;
    }

    public static VoiceChatDto ToDTO(this VoiceChat voiceChat, IMapper mapper)
    {
        var map = mapper.Map<VoiceChatDto>(voiceChat);

        return map;
    }

    public static IEnumerable<VoiceChatDto> ToDTOCollection(this IEnumerable<VoiceChat> voiceChats, IMapper mapper)
    {
        var map = mapper.Map<IEnumerable<VoiceChatDto>>(voiceChats);

        return map;
    }

    public static Expression<Func<VoiceChat, TValue>> ToExpression<TValue>(this Expression<Func<VoiceChatDto, TValue>> expression, IMapper mapper)
    {
        var map = mapper.MapExpression<Expression<Func<VoiceChat, TValue>>>(expression);

        return map;
    }
}
