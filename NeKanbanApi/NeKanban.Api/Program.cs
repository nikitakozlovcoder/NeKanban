using System.Text;
using Batteries.Exceptions;
using Batteries.FileStorage.DirectFileStorageAdapters;
using Batteries.FileStorage.FileStorageProviders;
using Batteries.FileStorage.FileStorageProxies;
using Batteries.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NeKanban.Data.Extensions;
using NeKanban.Data.Infrastructure;
using NeKanban.Logic.Configuration;
using NeKanban.Logic.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddDirectFileStorageAdapter();
builder.Services.AddWwwRootStorage(new WebRootStorageConfig
{
    Root = builder.Environment.WebRootPath,
    Folder = "storage",
    HostingUrl = builder.Configuration.GetValue<string>("HostingUrl")!
});

builder.Services.AddFileStorageProxy(new FileStorageProxyConfig
{
    ProxyEndpoint = "FileStorage/Proxy"
});
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
builder.Services.AddCors();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = jwt!.Audience,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt!.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.Secret!)),
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddAppIdentity();

var app = builder.Build();
app.Services.ValidateMapperConfiguration();
var shouldMigrate = app.Configuration.GetValue<bool>("MigrateOnStart");
if (shouldMigrate)
{
    using var scope = app.Services.CreateScope();
    var dbCtx = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbCtx.Migrate();
}
else
{
    using var scope = app.Services.CreateScope();
    var dbCtx = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    dbCtx.TestConnection();
}

app.UseCors(x=> x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseStaticFiles();
app.UseAppExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
