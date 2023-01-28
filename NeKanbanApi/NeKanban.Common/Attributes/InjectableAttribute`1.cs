namespace NeKanban.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class InjectableAttribute<T> : InjectableAttribute
{
    public InjectableAttribute() : base(typeof(T))
    {
    }
}