using AutoMapper;

namespace Financity.Application.Common.Mappings;

public interface IMapFrom<TSource>
{
    void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(TSource), GetType());
    }
}