﻿using AutoMapper;

namespace Batteries.Mapper.Interfaces;

public interface IAutoMapFrom<TSource, TDest> where TSource : class where TDest : class
{
    static abstract void ConfigureMap(IMappingExpression<TSource, TDest> cfg);
}