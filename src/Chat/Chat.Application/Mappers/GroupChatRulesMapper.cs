using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Chat.Application.DTOs;
using Chat.Domain.Entities;
using System.Linq.Expressions;

namespace Chat.Application.Mappers;

public static class GroupChatRulesMapper
{
    public static GroupChatRules ToEntity(this GroupChatRulesDto groupChatRulesDto, IMapper mapper)
    {
        var map = mapper.Map<GroupChatRules>(groupChatRulesDto);

        return map;
    }

    public static GroupChatRulesDto ToDTO(this GroupChatRules groupChatRules, IMapper mapper)
    {
        var map = mapper.Map<GroupChatRulesDto>(groupChatRules);

        return map;
    }

    public static IEnumerable<GroupChatRulesDto> ToDTOCollection(this IEnumerable<GroupChatRules> groupChatsRules, IMapper mapper)
    {
        var map = mapper.Map<IEnumerable<GroupChatRulesDto>>(groupChatsRules);

        return map;
    }

    public static Expression<Func<GroupChatRules, TValue>> ToExpression<TValue>(this Expression<Func<GroupChatRulesDto, TValue>> expression, IMapper mapper)
    {
        var map = mapper.MapExpression<Expression<Func<GroupChatRules, TValue>>>(expression);

        return map;
    }
}
