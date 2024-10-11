using FlexyBox.web;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        builder.Services.AddOidcAuthentication(options =>
        {
            options.ProviderOptions.Authority = builder.Configuration["Okta:Authority"];
            options.ProviderOptions.ClientId = builder.Configuration["Okta:ClientId"];
            options.ProviderOptions.RedirectUri = builder.Configuration["Okta:RedirectUri"];
            options.ProviderOptions.PostLogoutRedirectUri = builder.Configuration["Okta:PostLogoutRedirectUri"];
            options.ProviderOptions.ResponseType = "code";
        });



        await builder.Build().RunAsync();
    }
}