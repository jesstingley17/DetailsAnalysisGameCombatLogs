using Chat.Domain.Exceptions;

namespace Chat.Infrastructure.Exceptions;

public class EntityNotFoundException : DomainException
{
    public Type EntityType { get; }
    public object EntityId { get; }

    public EntityNotFoundException(Type entityType, object entityId)
        : base($"Entity '{entityType.Name}' with Id '{entityId}' was not found.")
    {
        EntityType = entityType;
        EntityId = entityId;
    }
}
