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

    public Task<NotificationDto> CreateAsync(NotificationDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(NotificationDto), $"The {nameof(NotificationDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        try
        {
            var rowsAffected = await _repository.DeleteAsync(id);

            return rowsAffected;
        }
        catch (ArgumentException ex)
        {
            return 0;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public async Task<IEnumerable<NotificationDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<NotificationDto>>(allData);

        return result;
    }

    public async Task<NotificationDto> GetByIdAsync(int id)
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

    public Task<int> UpdateAsync(NotificationDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(NotificationDto), $"The {nameof(NotificationDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<NotificationDto> CreateInternalAsync(NotificationDto item)
    {
        var map = _mapper.Map<Notification>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<NotificationDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(NotificationDto item)
    {
        var map = _mapper.Map<Notification>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
