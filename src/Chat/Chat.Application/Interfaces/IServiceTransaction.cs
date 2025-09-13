namespace Chat.Application.Interfaces;

public interface IServiceTransaction<TModel, TIdType> : IService<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    Task DeleteUseExistTransactionAsync(TIdType id);
}