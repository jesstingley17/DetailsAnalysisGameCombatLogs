using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using CombatAnalysis.NotificationBL.DTO;
using CombatAnalysis.NotificationBL.Interfaces;
using CombatAnalysis.NotificationDAL.Entities;
using CombatAnalysis.NotificationDAL.Interfaces;
using System.Linq.Expressions;

namespace CombatAnalysis.NotificationBL.Services;

internal class NotificationService(IGenericRepository<Notification, int> repository, IMapper mapper) : IService<NotificationDto, int>
{
    private readonly IGenericRepository<Notification, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<NotificationDto?> CreateAsync(NotificationDto item)
    {
        var map = _mapper.Map<Notification>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<NotificationDto>(createdItem);

        return resultMap;
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<NotificationDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<NotificationDto>>(allData);

        return result;
    }

    public async Task<NotificationDto?> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<NotificationDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<NotificationDto>> GetByParamAsync<TValue>(Expression<Func<NotificationDto, TValue>> property, TValue value)
    {
        var map = _mapper.MapExpression<Expression<Func<Notification, TValue>>>(property);
        var result = await _repository.GetByParamAsync(map, value);
        var resultMap = _mapper.Map<IEnumerable<NotificationDto>>(result);

        return resultMap;
    }

    public async Task UpdateAsync(NotificationDto item)
    {
        var map = _mapper.Map<Notification>(item);
        await _repository.UpdateAsync(map);
    }
}
