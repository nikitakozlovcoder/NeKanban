namespace Batteries.Exceptions;

public class EntityDoesNotExists: Exception
{
    public EntityDoesNotExists(Type entity) : base($"Entity {entity.Name} does not exists")
    {
    }
}