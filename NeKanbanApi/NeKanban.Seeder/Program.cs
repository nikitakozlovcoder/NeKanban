using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NeKanban.Data.Extensions;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Configuration;
using NeKanban.Logic.Seeders;

var host = Host.CreateDefaultBuilder().ConfigureServices((ctx, x) =>
{
    x.AddDbContext<ApplicationContext>(db =>
        db.UseNpgsql(ctx.Configuration.GetConnectionString("DefaultConnection")!));
    x.AddDataAccess();
    x.AddServices();
    x.AddAuthentication();
    x.AddAppIdentity();
}).Build();

var seeder = host.Services.GetRequiredService<ISeeder>();
seeder.Run(default).GetAwaiter().GetResult();
    