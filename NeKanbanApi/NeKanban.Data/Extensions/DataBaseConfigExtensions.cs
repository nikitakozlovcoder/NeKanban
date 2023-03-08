﻿using Batteries.Repository;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeKanban.Common.Entities;
using NeKanban.Data.Infrastructure;

namespace NeKanban.Data.Extensions;

public static class DataBaseConfigExtensions
{
    public static void AddDatabase(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ApplicationContext>(x =>
                x.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }
}