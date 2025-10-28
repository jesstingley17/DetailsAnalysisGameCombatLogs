using AutoMapper;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;

namespace CombatAnalysis.UserBL.Services;

internal class UserService(IUserRepository repository, IMapper mapper) : IUserService<AppUserDto>
{
    private readonly IUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AppUserDto?> CreateAsync(AppUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.Username)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.FirstName))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.FirstName)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.LastName))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.LastName)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<AppUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<AppUserDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(string id, AppUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.Username)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.FirstName))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.FirstName)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.LastName))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(AppUserDto.LastName)} of the {nameof(AppUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<AppUser>(item);
        await _repository.UpdateAsync(id, map);
    }

    public async Task DeleteAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<AppUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<AppUserDto>>(allData);

        return result;
    }

    public async Task<AppUserDto?> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<AppUserDto>(result);

        return resultMap;
    }

    public async Task<bool> CheckByUsernameAsync(string username)
    {
        var users = await GetAllAsync();
        var findByUsername = users.FirstOrDefault(x => x.Username == username);

        return findByUsername != null;
    }

    public async Task<AppUserDto?> FindByIdentityUserIdAsync(string identityUserId)
    {
        var result = await _repository.FindByIdentityUserIdAsync(identityUserId);
        var resultMap = _mapper.Map<AppUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<AppUserDto>> FindByUsernameStartAtAsync(string startAt)
    {
        var result = await _repository.FindByUsernameStartAtAsync(startAt);
        var resultMap = _mapper.Map<IEnumerable<AppUserDto>>(result);

        return resultMap;
    }
}
