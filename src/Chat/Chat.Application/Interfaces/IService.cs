namespace Chat.Application.Interfaces;

public interface IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task<TModel> CreateAsync(TModel item);

    Task UpdateAsync(TModel item);

    Task DeleteAsync(TIdType id);

    Task<IEnumerable<TModel>> GetAllAsync();

    Task<TModel> GetByIdAsync(TIdType id);
}
