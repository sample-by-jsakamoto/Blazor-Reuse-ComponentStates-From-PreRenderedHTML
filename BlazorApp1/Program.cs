using BlazorApp1;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
if (!builder.RootComponents.Any())
{
    builder.RootComponents.Add<App>("#app");
    builder.RootComponents.Add<HeadOutlet>("head::after");
}

ConfigureServices(builder.Services, new Uri(builder.HostEnvironment.BaseAddress));

await builder.Build().RunAsync();

static void ConfigureServices(IServiceCollection services, Uri baseAddress)
{
    services.AddScoped(sp => new HttpClient { BaseAddress = baseAddress });
}
