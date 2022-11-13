using System.Reflection;
using AutoMapper;

namespace Financity.Application.Common.Mappings;

public sealed class MappingProfile : Profile
{
    private readonly string _mapFromInterface = typeof(IMapFrom<>).Name;
    private readonly HashSet<Type> _mappingInterfaces = new() { typeof(IMapFrom<>), typeof(IMapTo<>) };
    private readonly string _mappingMethod;
    private readonly string _mapToInterface = typeof(IMapTo<>).Name;

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
            var customMappingMethod = type.GetMethod(_mappingMethod, BindingFlags.Public | BindingFlags.Static);
            if (customMappingMethod is not null)
            {
                customMappingMethod.Invoke(null, new[] { this });
            }
            else
            {
                var mappings = _mappingInterfaces.Select(i => type.GetInterface(i.Name))
                                                 .Where(t => t is not null)
                                                 .ToDictionary(
                                                     i => i.Name,
                                                     i => i.GetGenericArguments().FirstOrDefault()
                                                 );

                if (mappings.ContainsKey(_mapFromInterface)) CreateMap(mappings[_mapFromInterface], type);

                if (mappings.ContainsKey(_mapToInterface)) CreateMap(type, mappings[_mapToInterface]);
            }
        }
    }
}