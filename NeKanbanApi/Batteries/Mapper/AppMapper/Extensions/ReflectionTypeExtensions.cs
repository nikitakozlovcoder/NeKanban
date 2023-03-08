namespace Batteries.Mapper.AppMapper.Extensions;

public static class ReflectionTypeExtensions
{
    public static IEnumerable<Type> GetParents(this Type type)
    {
        var parent = type.BaseType;
        while (parent != null)
        {
            yield return parent;
            parent = parent.BaseType;
        }
    }
}