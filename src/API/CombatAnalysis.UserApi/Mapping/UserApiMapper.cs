using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserAPI.Models;

namespace CombatAnalysis.UserAPI.Mapping;

internal class UserApiMapper : Profile
{
    public UserApiMapper()
    {
        CreateMap<AppUserDto, AppUserModel>().ReverseMap();
        CreateMap<CustomerDto, CustomerModel>().ReverseMap();
        CreateMap<BannedUserDto, BannedUserModel>().ReverseMap();
        CreateMap<FriendCreateDto, FriendModel>().ReverseMap();
        CreateMap<RequestToConnectDto, RequestToConnectModel>().ReverseMap();
    }
}
