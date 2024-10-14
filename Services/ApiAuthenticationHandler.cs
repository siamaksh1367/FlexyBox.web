using System.Net.Http.Headers;

namespace FlexyBox.web.Services
{
    public class ApiAuthenticationHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;

        public ApiAuthenticationHandler(ITokenProvider tokenService)
        {
            _tokenProvider = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _tokenProvider.GetAccessTokenAsync();

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            Console.WriteLine("im executed send async");
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
