using AutoMapper;
using CombatAnalysis.NotificationAPI.Models;
using CombatAnalysis.NotificationBL.DTO;

namespace CombatAnalysis.NotificationAPI.Mapping;

internal class NotificationMapper : Profile
{
    public NotificationMapper()
    {
        CreateMap<NotificationDto, NotificationModel>().ReverseMap();
    }
}
