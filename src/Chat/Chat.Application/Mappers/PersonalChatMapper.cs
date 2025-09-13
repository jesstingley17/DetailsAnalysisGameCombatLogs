using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Chat.Application.DTOs;
using Chat.Domain.Aggregates;
using System.Linq.Expressions;

namespace Chat.Application.Mappers;

public static class PersonalChatMapper
{
    public static PersonalChat ToEntity(this PersonalChatDto personalChatDto, IMapper mapper)
    {
        var map = mapper.Map<PersonalChat>(personalChatDto);

        return map;
    }

    public static PersonalChatDto ToDTO(this PersonalChat personalChat, IMapper mapper)
    {
        var map = mapper.Map<PersonalChatDto>(personalChat);

        return map;
    }

    public static IEnumerable<PersonalChatDto> ToDTOCollection(this IEnumerable<PersonalChat> personalChats, IMapper mapper)
    {
        var map = mapper.Map<IEnumerable<PersonalChatDto>>(personalChats);

        return map;
    }

    public static Expression<Func<PersonalChat, TValue>> ToExpression<TValue>(this Expression<Func<PersonalChatDto, TValue>> expression, IMapper mapper)
    {
        var map = mapper.MapExpression<Expression<Func<PersonalChat, TValue>>>(expression);

        return map;
    }
}
