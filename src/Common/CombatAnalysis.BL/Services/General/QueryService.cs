using AutoMapper;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;

namespace CombatAnalysis.BL.Services.General;

internal class QueryService<TModel, TModelMap>(IGenericRepository<TModelMap> repository, IMapper mapper) : IQueryService<TModel>
    where TModel : class
    where TModelMap : class, IEntity
{
    private readonly IGenericRepository<TModelMap> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancellationToken)
    {
        var allData = await _repository.GetAllAsync(cancellationToken);
        var result = _mapper.Map<IEnumerable<TModel>>(allData);

        return result;
    }

    public async Task<TModel> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(id, 1, nameof(id));

        var result = await _repository.GetByIdAsync(id, cancellationToken);
        var resultMap = _mapper.Map<TModel>(result);

        return resultMap;
    }

    public async Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value, CancellationToken cancellationToken)
    {
        var result = await _repository.GetByParamAsync(paramName, value, cancellationToken);
        var resultMap = _mapper.Map<IEnumerable<TModel>>(result);

        return resultMap;
    }
}
