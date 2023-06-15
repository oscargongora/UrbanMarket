using Blazored.LocalStorage;
using ChicStreetwear.Client;
using FluentValidation;
using Fluxor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSassCompiler();
builder.Services.AddBlazoredLocalStorageAsSingleton();
builder.Services.AddMudServices();
builder.Services.AddFluxor(configure => configure.ScanAssemblies(typeof(Program).Assembly));

builder.AddHttpClients();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services
    .AddApiAuthorization();
//.AddAccountClaimsPrincipalFactory<ClientAccountClaimsPrincipalFactory>();

await builder.Build().RunAsync();
