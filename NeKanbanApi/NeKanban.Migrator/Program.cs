using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NeKanban.Data.Infrastructure;

var host = Host.CreateDefaultBuilder().ConfigureServices((ctx, x) =>
{
    x.AddDbContext<ApplicationContext>(db =>
        db.UseNpgsql(ctx.Configuration.GetConnectionString("DefaultConnection")!));
}).Build();

using var scope = host.Services.CreateScope();
var dbCtx = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
dbCtx.Migrate();