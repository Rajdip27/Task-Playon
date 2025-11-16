using System.Text;
using TaskPlayon.Domain.Model.Identities;
using TaskPlayon.Infrastructure.DatabaseContext;
using TaskPlayon.Infrastructure.Healper.Acls;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace TaskPlayon.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((s, builder) =>
        {
            // Ensure the correct namespace for 'UseSqlServer' is included
            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                   .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
            builder.UseLoggerFactory(s.GetRequiredService<ILoggerFactory>());
            builder.LogTo(Console.WriteLine, LogLevel.Debug);
        }, ServiceLifetime.Scoped);
        services.AddTransient<DapperApplicationDbContext>();
        services.AddIdentity<IdentityModel.User, IdentityModel.Role>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = false;
        }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
        services.AddTransient<ISignInHelper, SignInHelper>();

        services.AddHttpContextAccessor();
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
        });

        return services;
    }
}
