using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserDAL.Entities;

namespace CombatAnalysis.UserBL.Mapping;

public class UserBLMapper : Profile
{
    public UserBLMapper()
    {
        CreateMap<AppUserDto, AppUser>().ReverseMap(); ;
        CreateMap<BannedUserDto, BannedUser>().ReverseMap();
        CreateMap<CustomerDto, Customer>().ReverseMap();
        CreateMap<FriendDto, UserDAL.DTO.FriendDto>().ReverseMap();
        CreateMap<RequestToConnectDto, RequestToConnect>().ReverseMap();
    }
}