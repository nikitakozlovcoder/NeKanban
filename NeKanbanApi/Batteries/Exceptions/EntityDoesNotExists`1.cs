namespace Batteries.Exceptions;

public class EntityDoesNotExists<T> : EntityDoesNotExists
{
    public EntityDoesNotExists() : base(typeof(T))
    {
    }
}