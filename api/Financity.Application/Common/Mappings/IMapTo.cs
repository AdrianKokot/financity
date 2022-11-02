using AutoMapper;

namespace Financity.Application.Common.Mappings;

public interface IMapTo<TDestination>
{
    void Mapping(Profile profile)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("CreateMap <IMapTo>: " + GetType().Name + " -> " + typeof(TDestination).Name);
        Console.ForegroundColor = ConsoleColor.White;
        profile.CreateMap(GetType(), typeof(TDestination));
    }
}