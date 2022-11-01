using AutoMapper;

namespace Financity.Application.Common.Mappings;

public static class Projection
{
    public static MapperConfiguration For<TSource, TDestination>()
    {
        return new MapperConfiguration(cfg => cfg.CreateProjection<TSource, TDestination>());
    }
}