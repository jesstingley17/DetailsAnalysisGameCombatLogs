using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Chat.Application.DTOs;
using Chat.Domain.Entities;
using System.Linq.Expressions;

namespace Chat.Application.Mappers;

public static class GroupChatMessageMapper
{
    public static GroupChatMessage ToEntity(this GroupChatMessageDto groupChatMessageDto, IMapper mapper)
    {
        var map = mapper.Map<GroupChatMessage>(groupChatMessageDto);

        return map;
    }

    public static GroupChatMessageDto ToDTO(this GroupChatMessage groupChatMessage, IMapper mapper)
    {
        var map = mapper.Map<GroupChatMessageDto>(groupChatMessage);

        return map;
    }

    public static IEnumerable<GroupChatMessageDto> ToDTOCollection(this IEnumerable<GroupChatMessage> groupChatMessages, IMapper mapper)
    {
        var map = mapper.Map<IEnumerable<GroupChatMessageDto>>(groupChatMessages);

        return map;
    }

    public static IEnumerable<GroupChatMessageDto> ToDTOCollection(this IEnumerable<Domain.DTOs.GroupChatMessageDto> groupChatMessages, IMapper mapper)
    {
        var map = mapper.Map<IEnumerable<GroupChatMessageDto>>(groupChatMessages);

        return map;
    }

    public static Expression<Func<GroupChatMessage, TValue>> ToExpression<TValue>(this Expression<Func<GroupChatMessageDto, TValue>> expression, IMapper mapper)
    {
        var map = mapper.MapExpression<Expression<Func<GroupChatMessage, TValue>>>(expression);

        return map;
    }
}
