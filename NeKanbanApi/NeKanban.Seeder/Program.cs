using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NeKanban.Data.Extensions;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Configuration;
using NeKanban.Logic.Seeders;


var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false).Build();
var serviceProvider = new ServiceCollection();
serviceProvider.AddDbContext<ApplicationContext>(x =>
    x.UseNpgsql(config.GetConnectionString("DefaultConnection")!));
serviceProvider.AddDataAccess();
serviceProvider.AddAppIdentity();
serviceProvider.AddServices();
var serviceCollection = serviceProvider.BuildServiceProvider();
var seeder = serviceCollection.GetRequiredService<ISeeder>();

seeder.Run(default).GetAwaiter().GetResult();
    