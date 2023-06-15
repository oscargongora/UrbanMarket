using ChicStreetwear.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace ChicStreetwear.Client;

public static class DependencyInjection
{
    public static void AddHttpClients(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddHttpClient("ChicStreetwear.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)).AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        // Supply HttpClient instances that include access tokens when making requests to the server project
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ChicStreetwear.ServerAPI"));
        builder.Services.AddScoped<FileService>();
    }
}
