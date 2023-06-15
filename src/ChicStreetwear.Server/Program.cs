using ChicStreetwear.Application;
using ChicStreetwear.Infrastructure;
using ChicStreetwear.Server;
using ChicStreetwear.Server.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

string? authority = builder.Configuration.GetValue<string>("JwtBearerOptions:Authority");

if (authority is not null)
{
    builder.Services.Configure<JwtBearerOptions>(
        IdentityServerJwtConstants.IdentityServerJwtBearerScheme,
        options =>
        {
            options.Authority = authority;
        });
}

builder.Services.Configure<StripeOptions>(builder.Configuration.GetSection(StripeOptions.SECTION_NAME));

// Add services to the container.
builder.Services.AddApplicationServices()
                .AddInfrastructureServices(builder.Configuration)
                .AddApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //default
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
    ////////////////////////////////
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    //default
    app.UseExceptionHandler("/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();

//app.UseExceptionHandler("/error");

app.UseIdentityServer();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapBlazorHub();

app.MapFallbackToFile("index.html");

await app.AddDefaultAdministratorUserFromConfiguration();

app.Run();
