using ChicStreetwear.Application.Common.Interfaces;
using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Infrastructure.Files;
using ChicStreetwear.Infrastructure.Identity;
using ChicStreetwear.Infrastructure.Options;
using ChicStreetwear.Infrastructure.Persistence.Repositories;
using ChicStreetwear.Infrastructure.Services;
using ChicStreetwear.Infrastructure.Settings;
using ChicStreetwear.Shared.Identity;
using ChicStreetwear.Shared.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace ChicStreetwear.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
                                                       ConfigurationManager configurationManager)
    {
        services.AddStorageServices(configurationManager);
        services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddPersistenceServices(configurationManager);
        services.AddIdentityServices(configurationManager);
        return services;
    }

    private static IServiceCollection AddStorageServices(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.Configure<CloudStorageOptions>(configurationManager.GetSection(CloudStorageOptions.SECTION_NAME));
        services.AddScoped<IStorageService, StorageService>();
        return services;
    }

    private static IServiceCollection AddPersistenceServices(this IServiceCollection services, ConfigurationManager configurationManager)
    {

        //services.AddDbContext<ChicStreetwearDbContext>(options =>
        //{
        //    options.UseSqlServer(configurationManager.GetConnectionString(DbSettings.ConnectionStrings.ChicStreetwearDbContext));
        //});

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductReviewRepository, ProductReviewRepository>();

        return services;
    }

    private static IServiceCollection AddIdentityServices(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        var connectionString = configurationManager.GetConnectionString(DbSettings.ConnectionStrings.ChicStreetwearIdentityDbContext) ??
            throw new InvalidOperationException($"Connection string '{DbSettings.ConnectionStrings.ChicStreetwearIdentityDbContext}' not found.");

        services.AddDbContext<ChicStreetwearIdentityDbContext>(options => options.UseSqlServer(connectionString));

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ChicStreetwearIdentityDbContext>();

        services.AddIdentityServer(options =>
        {
            options.LicenseKey = configurationManager.GetValue<string>("DuendeLicenseKey");
        })
        .AddApiAuthorization<ApplicationUser, ChicStreetwearIdentityDbContext>(configureApi =>
        {
            configureApi.IdentityResources["openid"].UserClaims.Add("name");
            configureApi.ApiResources.Single().UserClaims.Add("name");
            configureApi.IdentityResources["openid"].UserClaims.Add("role");
            configureApi.ApiResources.Single().UserClaims.Add("role");
        });

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");

        services.AddAuthentication().AddIdentityServerJwt();

        return services;
    }

    public async static Task AddDefaultAdministratorUserFromConfiguration(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationRole>>() ??
            throw new ArgumentNullException("Role manager is null");

        if (await roleManager.RoleExistsAsync(IdentityDefaults.ADMINISTRATOR_ROLE) == false)
        {
            await roleManager.CreateAsync(new ApplicationRole(IdentityDefaults.ADMINISTRATOR_ROLE));
        }

        var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>() ??
            throw new ArgumentNullException("User manager is null");

        var defaultAdministratorSection = application.Configuration.GetSection(IdentityDefaults.DEFAULT_ADMINISTRATOR_SECTION);

        if (!defaultAdministratorSection.Exists())
        {
            throw new Exception($"The {IdentityDefaults.DEFAULT_ADMINISTRATOR_SECTION} section does not exist.");
        }

        var defaultAdministrator = defaultAdministratorSection.Get<DefaultAdministratorUserSection>();

        if (defaultAdministrator is not null)
        {
            if (await userManager.FindByNameAsync(defaultAdministrator.UserName) is null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = defaultAdministrator.UserName,
                    Email = defaultAdministrator.Email,
                };
                var createUserResult = await userManager.CreateAsync(newUser);
                if (createUserResult.Succeeded)
                {
                    await userManager.AddPasswordAsync(newUser, defaultAdministrator.Password);
                    await userManager.AddToRoleAsync(newUser, IdentityDefaults.ADMINISTRATOR_ROLE);
                }
            }
        }
    }
}
