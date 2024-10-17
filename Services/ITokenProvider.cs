using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace FlexyBox.web.Services
{
    public interface ITokenProvider
    {
        Task<string> GetAccessTokenAsync();
    }

    public class TokenProvider : ITokenProvider
    {
        private readonly IAccessTokenProvider _accessTokenProvider;

        public TokenProvider(IAccessTokenProvider accessTokenProvider)
        {
            _accessTokenProvider = accessTokenProvider;
        }
        public async Task<string> GetAccessTokenAsync()
        {
            var tokenResult = await _accessTokenProvider.RequestAccessToken(new AccessTokenRequestOptions());

            var accessTokenRetrieved = tokenResult.TryGetToken(out var token);
            if (accessTokenRetrieved)
                return token.Value;
            return string.Empty;
        }
    }
}
