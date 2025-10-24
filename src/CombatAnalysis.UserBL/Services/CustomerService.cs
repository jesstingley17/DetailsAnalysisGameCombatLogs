using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysis.UserDAL.Entities;
using CombatAnalysis.UserDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.UserBL.Services;

internal class CustomerService(IGenericRepository<Customer, string> repository, IMapper mapper) : IService<CustomerDto, string>
{
    private readonly IGenericRepository<Customer, string> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<CustomerDto?> CreateAsync(CustomerDto item)
    {
        if (string.IsNullOrEmpty(item.City))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(CustomerDto.City)} of the {nameof(CustomerDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.Country))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(CustomerDto.Country)} of the {nameof(CustomerDto)} object can't be null or empty");
        }

        var map = _mapper.Map<Customer>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<CustomerDto>(createdItem);

        return resultMap;
    }

    public async Task UpdateAsync(CustomerDto item)
    {
        if (string.IsNullOrEmpty(item.City))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(CustomerDto.City)} of the {nameof(CustomerDto)} object can't be null or empty");
        }

        if (string.IsNullOrEmpty(item.Country))
        {
            throw new ArgumentNullException(nameof(CustomerDto),
                $"The property {nameof(CustomerDto.Country)} of the {nameof(CustomerDto)} object can't be null or empty");
        }

        var map = _mapper.Map<Customer>(item);
        await _repository.UpdateAsync(map);
    }

    public async Task DeleteAsync(string id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<List<CustomerDto>>(allData);

        return result;
    }

    public async Task<CustomerDto?> GetByIdAsync(string id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<CustomerDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<CustomerDto>> GetByParamAsync<TValue>(Expression<Func<CustomerDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<Customer, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<CustomerDto>>(result);

        return resultMap;
    }
}
