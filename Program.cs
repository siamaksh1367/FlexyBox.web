using FlexyBox.contract.Services;
using FlexyBox.web;
using FlexyBox.web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Polly;
using Polly.Extensions.Http;

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
            options.ProviderOptions.DefaultScopes.Add("openid");
            options.ProviderOptions.DefaultScopes.Add("profile");
            options.ProviderOptions.DefaultScopes.Add("email");
            options.ProviderOptions.ResponseType = "code";
            options.ProviderOptions.AdditionalProviderParameters.Add("audience", "https://localhost:7204/");
        });

        builder.Services.AddHttpClient("FlexyBox", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Api:BaseAddress"]);
        }).AddPolicyHandler(GetRetryPolicy());

        builder.Services.AddTransient<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        builder.Services.AddScoped<HttpRequestBuilder>();
        await builder.Build().RunAsync();
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}