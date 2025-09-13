using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Chat.Application.DTOs;
using Chat.Domain.Aggregates;
using System.Linq.Expressions;

namespace Chat.Application.Mappers;

public static class GroupChatMapper
{
    public static GroupChat ToEntity(this GroupChatDto groupChatDto, IMapper mapper)
    {
        var map = mapper.Map<GroupChat>(groupChatDto);

        return map;
    }

    public static GroupChatDto ToDTO(this GroupChat groupChat, IMapper mapper)
    {
        var map = mapper.Map<GroupChatDto>(groupChat);

        return map;
    }

    public static IEnumerable<GroupChatDto> ToDTOCollection(this IEnumerable<GroupChat> groupChats, IMapper mapper)
    {
        var map = mapper.Map<IEnumerable<GroupChatDto>>(groupChats);

        return map;
    }

    public static Expression<Func<GroupChat, TValue>> ToExpression<TValue>(this Expression<Func<GroupChatDto, TValue>> expression, IMapper mapper)
    {
        var map = mapper.MapExpression<Expression<Func<GroupChat, TValue>>>(expression);

        return map;
    }
}
