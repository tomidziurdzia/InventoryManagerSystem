using Application.Extension.Identity;
using Application.Interface.Identity;
using Infrastructure.DataAccess;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection;

public static class ServiceContainer
{
    public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(o => o.UseNpgsql(config.GetConnectionString("Default")),
            ServiceLifetime.Scoped);

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        services.AddAuthorizationBuilder()
            .AddPolicy("AdministrationPolicy", adp =>
            {
                adp.RequireAuthenticatedUser();
                adp.RequireRole("Admin", "Manager");
            })
            .AddPolicy("UserPolicy", adp =>
            {
                adp.RequireAuthenticatedUser();
                adp.RequireRole("User");
            });
        services.AddCascadingAuthenticationState();
        services.AddScoped<Application.Interface.Identity.IAccount, Account>();
        return services;
    }
}