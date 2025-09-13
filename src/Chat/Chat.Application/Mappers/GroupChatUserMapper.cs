using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Chat.Application.DTOs;
using Chat.Domain.Entities;
using System.Linq.Expressions;

namespace Chat.Application.Mappers;

public static class GroupChatUserMapper
{
    public static GroupChatUser ToEntity(this GroupChatUserDto groupChatUserDto, IMapper mapper)
    {
        var map = mapper.Map<GroupChatUser>(groupChatUserDto);

        return map;
    }

    public static GroupChatUserDto ToDTO(this GroupChatUser groupChatUser, IMapper mapper)
    {
        var map = mapper.Map<GroupChatUserDto>(groupChatUser);

        return map;
    }

    public static IEnumerable<GroupChatUserDto> ToDTOCollection(this IEnumerable<GroupChatUser> groupChatUsers, IMapper mapper)
    {
        var map = mapper.Map<IEnumerable<GroupChatUserDto>>(groupChatUsers);

        return map;
    }

    public static Expression<Func<GroupChatUser, TValue>> ToExpression<TValue>(this Expression<Func<GroupChatUserDto, TValue>> expression, IMapper mapper)
    {
        var map = mapper.MapExpression<Expression<Func<GroupChatUser, TValue>>>(expression);

        return map;
    }
}
