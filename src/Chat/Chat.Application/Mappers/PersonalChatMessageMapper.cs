using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Chat.Application.DTOs;
using Chat.Domain.Entities;
using System.Linq.Expressions;

namespace Chat.Application.Mappers;

public static class PersonalChatMessageMapper
{
    public static PersonalChatMessage ToEntity(this PersonalChatMessageDto personalChatMessageDto, IMapper mapper)
    {
        var map = mapper.Map<PersonalChatMessage>(personalChatMessageDto);

        return map;
    }

    public static PersonalChatMessageDto ToDTO(this PersonalChatMessage personalChatMessage, IMapper mapper)
    {
        var map = mapper.Map<PersonalChatMessageDto>(personalChatMessage);

        return map;
    }

    public static IEnumerable<PersonalChatMessageDto> ToDTOCollection(this IEnumerable<PersonalChatMessage> personalChatMessages, IMapper mapper)
    {
        var map = mapper.Map<IEnumerable<PersonalChatMessageDto>>(personalChatMessages);

        return map;
    }

    public static Expression<Func<PersonalChatMessage, TValue>> ToExpression<TValue>(this Expression<Func<PersonalChatMessageDto, TValue>> expression, IMapper mapper)
    {
        var map = mapper.MapExpression<Expression<Func<PersonalChatMessage, TValue>>>(expression);

        return map;
    }
}
