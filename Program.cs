using Blazored.Toast;
using FlexyBox.contract.Services;
using FlexyBox.web;
using FlexyBox.web.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Polly;
using Polly.Extensions.Http;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.HostEnvironment.Environment}.json", optional: true, reloadOnChange: true);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

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
            options.UserOptions.RoleClaim = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        });
        builder.Services.AddBlazoredToast();


        builder.Services.AddTransient<ApiAuthenticationHandler>();
        builder.Services.AddHttpClient("FlexyBox", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["Api:BaseAddress"]);
        }).AddPolicyHandler(GetRetryPolicy())
        .AddHttpMessageHandler<ApiAuthenticationHandler>();


        builder.Services.AddScoped(typeof(AccountClaimsPrincipalFactory<RemoteUserAccount>), typeof(CustomAccountFactory));
        builder.Services.AddTransient<ICategoryService, CategoryService>();
        builder.Services.AddTransient<ITagService, TagService>();
        builder.Services.AddTransient<IPostService, PostService>();
        builder.Services.AddTransient<ICommentService, CommentService>();

        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        builder.Services.AddTransient<HttpRequestBuilder>();
        await builder.Build().RunAsync();
    }

    static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}