using System.Reflection;
using AutoMapper;
using NeKanban.Common.Interfaces;

namespace NeKanban.Logic.Configuration;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();
                foreach (var typeInterface in interfaces)
                {
                    if (!(typeInterface.IsGenericType && typeInterface.GetGenericTypeDefinition() == typeof(IMapSrcDest<,>)))
                    {
                        continue;
                    }

                    var src = typeInterface.GetGenericArguments()[0];
                    var dst = typeInterface.GetGenericArguments()[1];
                    if (dst != type)
                    {
                        throw new Exception($"{type} can`t define map configuration for {src}");
                    }
                    
                    var createMap = typeof(MappingProfile).GetMethods().Single(x => x is {Name: "CreateMap", IsGenericMethod: true} && x.GetGenericArguments().Length == 2 && x.GetParameters().Length == 0);
                    createMap = createMap.MakeGenericMethod(src, dst);
                    var config = createMap.Invoke(this, null)!;
                    type.GetMethods().Single(x =>
                        x is {Name: "ConfigureMap", IsGenericMethod: false} && x.GetParameters().Length == 1 
                                                                            && x.GetParameters()[0].ParameterType.GenericTypeArguments[0] == src).Invoke(null, new[] {config});
                }
            }
        }
    }
}