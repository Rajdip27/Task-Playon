using BizCommerce.Application;
using BizCommerce.Application.Behavior;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Text.Json.Serialization;
using TaskPlayon.Application.AuthServices;
using TaskPlayon.Application.Behavior;
using TaskPlayon.Application.Common;
using TaskPlayon.Application.FileServices;
using TaskPlayon.Application.Logging;
using TaskPlayon.Application.MapperConfiguration;
using TaskPlayon.Application.Repositories;
using TaskPlayon.Application.Repositories.Base;
using TaskPlayon.Application.Services;
using TaskPlayon.Domain.Model.Auth;

namespace TaskPlayon.Application;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        

        services.Scan(scan => scan.FromAssemblyOf<IApplication>()
       .AddClasses(classes => classes.AssignableTo<IApplication>())
       .AddClasses(x => x.AssignableTo(typeof(IBaseRepository<,,>)))
       .AsSelfWithInterfaces()
       .WithScopedLifetime());
        services.AddScoped(typeof(IAppLogger<>), typeof(AppLogger<>));

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
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IInvoiceRepository, InvoiceRepository>();
        services.AddTransient<IPdfService, PdfService>();
        
        services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddHttpClient();
        //services.AddMemoryCache();
        //services.AddSingleton<ICacheService, MemoryCacheService>();
    }
}
