using System.Reflection;
using AutoMapper;

namespace Financity.Application.Common.Mappings;

public sealed class MappingProfile : Profile
{
    private readonly HashSet<Type> _mappingInterfaces = new() { typeof(IMapFrom<>), typeof(IMapTo<>) };
    private readonly string _mappingMethod;

    public MappingProfile()
    {
        _mappingMethod = _mappingInterfaces.First().GetMethods().First().Name;
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
                            .Where(t => !t.IsGenericType && !t.IsInterface && t.GetInterfaces().Any(i =>
                                i.IsGenericType && _mappingInterfaces.Contains(i.GetGenericTypeDefinition()))
                            );

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var methodInfo = type.GetMethod(_mappingMethod)
                             ?? _mappingInterfaces.Select(interfaceType =>
                                                      type.GetInterface(interfaceType.Name)?.GetMethod(_mappingMethod)
                                                  )
                                                  .FirstOrDefault(x => x is not null);

            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}