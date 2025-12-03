namespace Chat.Domain.Interfaces;

public interface IRepositoryEntity<TId>
    where TId : notnull
{
    TId Id { get; }
}
