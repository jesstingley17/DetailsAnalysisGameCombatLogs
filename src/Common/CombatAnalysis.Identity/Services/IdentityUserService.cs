using AutoMapper;
using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;

namespace CombatAnalysis.Identity.Services;

internal class IdentityUserService(IIdentityUserRepository identityUserRepository, IMapper mapper) : IIdentityUserService
{
    private readonly IIdentityUserRepository _identityUserRepository = identityUserRepository;
    private readonly IMapper _mapper = mapper;

    public async Task CreateAsync(IdentityUserDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<IdentityUser>(item);
        await _identityUserRepository.CreateAsync(map);
    }

    public async Task<int> UpdateAsync(string id, IdentityUserDto item)
    {
        CheckParams(item);

        var map = _mapper.Map<IdentityUser>(item);
        return await _identityUserRepository.UpdateAsync(id, map);
    }

    public async Task<IdentityUserDto> GetByIdAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);

        var identityUser = await _identityUserRepository.GetByIdAsync(id);
        var map = _mapper.Map<IdentityUserDto>(identityUser);

        return map;
    }

    public async Task<bool> CheckByEmailAsync(string email)
    {
        var userPresent = await _identityUserRepository.CheckByEmailAsync(email);

        return userPresent;
    }

    public async Task<IdentityUserDto> GetByEmailAsync(string email)
    {
        var identityUser = await _identityUserRepository.GetAsync(email);
        var map = _mapper.Map<IdentityUserDto>(identityUser);

        return map;
    }

    private static void CheckParams(IdentityUserDto item)
    {
        ArgumentException.ThrowIfNullOrEmpty(item.Id, nameof(item.Id));

        ArgumentException.ThrowIfNullOrEmpty(item.Email, nameof(item.Email));
        ArgumentException.ThrowIfNullOrEmpty(item.PasswordHash, nameof(item.PasswordHash));
        ArgumentException.ThrowIfNullOrEmpty(item.Salt, nameof(item.Salt));
    }
}
