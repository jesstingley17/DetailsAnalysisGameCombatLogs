using Chat.Domain.Enums;
using Chat.Domain.Exceptions;

namespace Chat.Infrastructure.Exceptions;

public class EntityNotFoundException(Type entityType, object entityId) : DomainException($"Entity '{entityType.Name}' with Id '{entityId}' was not found.", ExceptionCode.NotFound)
{
    public Type EntityType { get; } = entityType;

    public object EntityId { get; } = entityId;
}