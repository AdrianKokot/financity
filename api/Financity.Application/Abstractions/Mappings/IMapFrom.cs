using AutoMapper;

namespace Financity.Application.Abstractions.Mappings;

public interface IMapFrom<TSource>
{
    static void CreateMap(Profile profile)
    {
    }
}