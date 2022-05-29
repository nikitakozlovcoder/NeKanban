using Microsoft.EntityFrameworkCore;
using NeKanban.Data;
using Npgsql;

namespace NeKanban.Extensions;

public static class DataBaseConfigExtensions
{
    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        if (string.IsNullOrEmpty(databaseUrl))
        {
            builder.Services.AddDbContext<ApplicationContext>(x =>
                x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            return;
        }
        
        var databaseUri = new Uri(databaseUrl);
        var userInfo = databaseUri.UserInfo.Split(':');
        var npgsqlConnectionStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = databaseUri.Host,
            Port = databaseUri.Port,
            Username = userInfo[0],
            Password = userInfo[1],
            Database = databaseUri.LocalPath.TrimStart('/')
        };
        
        builder.Services.AddDbContext<ApplicationContext>(x =>
            x.UseNpgsql(npgsqlConnectionStringBuilder.ToString()));
    }
}