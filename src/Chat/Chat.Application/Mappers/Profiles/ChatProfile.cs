using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Mappers.Profiles;

public class ChatProfile : Profile
{
    public ChatProfile()
    {
        ValueObjectMap();

        CreateMap<VoiceChatDto, VoiceChat>()
                 .ConstructUsing(dto => new VoiceChat(
                     dto.AppUserId
                 )).ReverseMap();

        CreateMap<PersonalChatDto, PersonalChat>()
                 .ConstructUsing(dto => new PersonalChat(
                     dto.InitiatorId,
                     dto.CompanionId,
                     dto.InitiatorUnreadMessages ?? 0,
                     dto.CompanionUnreadMessages ?? 0
                 )).ReverseMap();

        CreateMap<PersonalChatMessageDto, PersonalChatMessage>()
                 .ConstructUsing(dto => new PersonalChatMessage(
                     dto.Username,
                     dto.Message,
                     dto.Time,
                     dto.PersonalChatId,
                     dto.AppUserId,
                     dto.Status,
                     dto.Type,
                     dto.MarkedType
                 )).ReverseMap();

        CreateMap<GroupChatDto, GroupChat>()
                 .ConstructUsing(dto => new GroupChat(
                     dto.Name,
                     dto.OwnerId
                 )).ReverseMap();

        CreateMap<GroupChatRulesDto, GroupChatRules>()
                 .ConstructUsing(dto => new GroupChatRules(
                     dto.GroupChatId,
                     dto.InvitePeople,
                     dto.RemovePeople,
                     dto.PinMessage,
                     dto.Announcements
                 )).ReverseMap();

        CreateMap<GroupChatMessageDto, GroupChatMessage>()
                 .ConstructUsing(dto => new GroupChatMessage(
                     dto.Username,
                     dto.Message,
                     dto.GroupChatId,
                     dto.GroupChatUserId,
                     dto.Status,
                     dto.Type,
                     dto.MarkedType
                 )).ReverseMap();
        
        CreateMap<GroupChatUserDto, GroupChatUser>()
                 .ConstructUsing(dto => new GroupChatUser(
                     dto.Username,
                     dto.GroupChatId,
                     dto.AppUserId,
                     dto.UnreadMessages
                 )).ReverseMap();
    }

    private void ValueObjectMap()
    {
        CreateMap<GroupChatId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, GroupChatId>()
            .ConvertUsing(src => new GroupChatId(src));

        CreateMap<GroupChatUserId, string>()
            .ConvertUsing(src => src.Value);

        CreateMap<string, GroupChatUserId>()
            .ConvertUsing(src => new GroupChatUserId(src));

        CreateMap<GroupChatMessageId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, GroupChatMessageId>()
            .ConvertUsing(src => new GroupChatMessageId(src));

        CreateMap<GroupChatRulesId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, GroupChatRulesId>()
            .ConvertUsing(src => new GroupChatRulesId(src));

        CreateMap<PersonalChatId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, PersonalChatId>()
            .ConvertUsing(src => new PersonalChatId(src));

        CreateMap<PersonalChatMessageId, int>()
            .ConvertUsing(src => src.Value);

        CreateMap<int, PersonalChatMessageId>()
            .ConvertUsing(src => new PersonalChatMessageId(src));

        CreateMap<UserId, string>()
            .ConvertUsing(src => src.Value);

        CreateMap<string, UserId>()
            .ConvertUsing(src => new UserId(src));


    }
}
