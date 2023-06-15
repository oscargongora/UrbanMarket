using ChicStreetwear.Server.Services;
using Fluxor;

namespace ChicStreetwear.Server;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddRazorPages();
        services.AddServerSideBlazor();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddFluxor(configure =>
        {
            configure.ScanAssemblies(typeof(Program).Assembly);
#if DEBUG
            configure.UseReduxDevTools();
#endif
        });
        services.AddScoped<CartService>();
        return services;
    }
}
