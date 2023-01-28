namespace NeKanban.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class InjectableAttribute : Attribute
{
    public Type[] Abstractions { get; }
    public InjectableAttribute(params Type[] abstractions)
    {
        Abstractions = abstractions;
    }
}