﻿using AutoMapper;

namespace Financity.Application.Common.Mappings;

public interface IMapFrom<TSource>
{
    void Mapping(Profile profile)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("CreateMap <IMapFrom>: " + GetType().Name + " -> " + typeof(TSource).Name);
        Console.ForegroundColor = ConsoleColor.White;
        profile.CreateMap(typeof(TSource), GetType());
    }
}