using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Aggregates;
using Chat.Domain.Entities;

namespace Chat.Application.Mappers.Profiles;

public class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<VoiceChatDto, VoiceChat>()
                 .ConstructUsing(dto => new VoiceChat(
                     dto.Id,
                     dto.AppUserId
                 )).ReverseMap();
        CreateMap<PersonalChatDto, PersonalChat>()
                 .ConstructUsing(dto => new PersonalChat(
                     dto.InitiatorId,
                     dto.CompanionId,
                     dto.InitiatorUnreadMessages,
                     dto.CompanionUnreadMessages
                 )).ReverseMap();
        CreateMap<PersonalChatMessageDto, PersonalChatMessage>()
                 .ConstructUsing(dto => new PersonalChatMessage(
                     dto.Username,
                     dto.Message,
                     dto.ChatId,
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
                     dto.ChatId,
                     dto.InvitePeople,
                     dto.RemovePeople,
                     dto.PinMessage,
                     dto.Announcements
                 )).ReverseMap();
        CreateMap<GroupChatMessageDto, GroupChatMessage>()
                 .ConstructUsing(dto => new GroupChatMessage(
                     dto.Username,
                     dto.Message,
                     dto.ChatId,
                     dto.GroupChatUserId,
                     dto.Status,
                     dto.Type,
                     dto.MarkedType
                 )).ReverseMap();
        CreateMap<GroupChatUserDto, GroupChatUser>()
                 .ConstructUsing(dto => new GroupChatUser(
                     dto.Id,
                     dto.Username,
                     dto.ChatId,
                     dto.AppUserId,
                     dto.UnreadMessages
                 )).ReverseMap();
        CreateMap<GroupChatUserDto, GroupChatUser>().ReverseMap();
    }
}
