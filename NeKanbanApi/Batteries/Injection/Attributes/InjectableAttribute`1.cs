namespace Batteries.Injection.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class InjectableAttribute<T> : InjectableAttribute
{
    public InjectableAttribute() : base(typeof(T))
    {
    }
}