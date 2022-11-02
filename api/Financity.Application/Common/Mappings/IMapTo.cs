using AutoMapper;

namespace Financity.Application.Common.Mappings;

public interface IMapTo<TDestination>
{
    void Mapping(Profile profile)
    {
        profile.CreateMap(GetType(), typeof(TDestination));
    }
}