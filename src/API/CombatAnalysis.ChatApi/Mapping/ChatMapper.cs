using AutoMapper;
using Chat.Application.DTOs;
using CombatAnalysis.ChatApi.Models;

namespace CombatAnalysis.ChatApi.Mapping;

internal class ChatMapper : Profile
{
    public ChatMapper()
    {
        CreateMap<VoiceChatDto, VoiceChatModel>().ReverseMap();
        CreateMap<PersonalChatDto, PersonalChatModel>().ReverseMap();
        CreateMap<PersonalChatMessageDto, PersonalChatMessageModel>().ReverseMap();
        CreateMap<GroupChatDto, GroupChatModel>().ReverseMap();
        CreateMap<GroupChatRulesDto, GroupChatRulesModel>().ReverseMap();
        CreateMap<GroupChatMessageDto, GroupChatMessageModel>().ReverseMap();
        CreateMap<GroupChatUserDto, GroupChatUserModel>().ReverseMap();
    }
}
