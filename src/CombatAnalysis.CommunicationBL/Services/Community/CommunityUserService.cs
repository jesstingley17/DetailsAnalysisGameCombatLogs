using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.CommunicationBL.DTO.Community;
using CombatAnalysis.CommunicationBL.Interfaces;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.CommunicationBL.Services.Community;

internal class CommunityUserService : IService<CommunityUserDto, string>
{
    private readonly IGenericRepository<CommunityUser, string> _repository;
    private readonly IMapper _mapper;

    public CommunityUserService(IGenericRepository<CommunityUser, string> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<CommunityUserDto> CreateAsync(CommunityUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityUserDto), $"The {nameof(CommunityUserDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(string id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<CommunityUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<CommunityUserDto>>(allData);

        return result;
    }

    public async Task<CommunityUserDto> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CommunityUserDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CommunityUserDto>> GetByParamAsync<TValue>(Expression<Func<CommunityUserDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<CommunityUser, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<CommunityUserDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(CommunityUserDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(CommunityUserDto), $"The {nameof(CommunityUserDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<CommunityUserDto> CreateInternalAsync(CommunityUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(CommunityUserDto),
                $"The property {nameof(CommunityUserDto.Username)} of the {nameof(CommunityUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunityUser>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CommunityUserDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(CommunityUserDto item)
    {
        if (string.IsNullOrEmpty(item.Username))
        {
            throw new ArgumentNullException(nameof(CommunityUserDto),
                $"The property {nameof(CommunityUserDto.Username)} of the {nameof(CommunityUserDto)} object can't be null or empty");
        }

        var map = _mapper.Map<CommunityUser>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
