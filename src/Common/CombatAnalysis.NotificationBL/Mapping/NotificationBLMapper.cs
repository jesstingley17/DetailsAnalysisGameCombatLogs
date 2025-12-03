using AutoMapper;
using CombatAnalysis.NotificationBL.DTO;
using CombatAnalysis.NotificationDAL.Entities;

namespace CombatAnalysis.NotificationBL.Mapping;

public class NotificationBLMapper : Profile
{
    public NotificationBLMapper()
    {
        CreateMap<NotificationDto, Notification>().ReverseMap();
    }
}
