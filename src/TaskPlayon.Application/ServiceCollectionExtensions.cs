using System.Text.Json.Serialization;
using TaskPlayon.Application.AuthServices;
using TaskPlayon.Application.Behavior;
using TaskPlayon.Application.Common;
using TaskPlayon.Application.FileServices;
using TaskPlayon.Application.MapperConfiguration;
using TaskPlayon.Application.Repositories.Base;
using TaskPlayon.Domain.Model.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using BizCommerce.Application;
using BizCommerce.Application.Behavior;

namespace TaskPlayon.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(config =>
        {
            var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();
            config.Filters.Add(new AuthorizeFilter(policy));

        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.Scan(scan => scan.FromAssemblyOf<IApplication>()
       .AddClasses(classes => classes.AssignableTo<IApplication>())
       .AddClasses(x => x.AssignableTo(typeof(IBaseRepository<,,>)))
       .AsSelfWithInterfaces()
       .WithScopedLifetime());

        //services.AddValidatorsFromAssembly(typeof(IApplication).Assembly);

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblies(typeof(IApplication).Assembly);
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            x.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        });

        services.AddAutoMapper(x => {
            x.AddMaps(typeof(IApplication).Assembly);

        });
        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IFileService, FileService>();
        
        services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddHttpClient();
        //services.AddMemoryCache();
        //services.AddSingleton<ICacheService, MemoryCacheService>();
    }
}
