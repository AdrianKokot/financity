using System.Reflection;
using AutoMapper;

namespace Financity.Application.Common.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes().Where(p =>
                p.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
            )
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            type.GetMethod("Mapping")?.Invoke(instance, new object[] { this });
        }
    }
}