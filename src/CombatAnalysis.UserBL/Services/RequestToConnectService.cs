using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.UserBL.Services;

internal class RequestToConnectService(IGenericRepository<RequestToConnect, int> repository, IMapper mapper) : IService<RequestToConnectDto, int>
{
    private readonly IGenericRepository<RequestToConnect, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<RequestToConnectDto?> CreateAsync(RequestToConnectDto item)
    {
        var map = _mapper.Map<RequestToConnect>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<RequestToConnectDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(RequestToConnectDto item)
    {
        var map = _mapper.Map<RequestToConnect>(item);
        await _repository.UpdateAsync(item.Id, map);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<RequestToConnectDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<RequestToConnectDto>>(allData);

        return result;
    }

    public async Task<RequestToConnectDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<RequestToConnectDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<RequestToConnectDto>> GetByParamAsync<TValue>(Expression<Func<RequestToConnectDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<RequestToConnect, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<RequestToConnectDto>>(result);

        return resultMap;
    }
}
