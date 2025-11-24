using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
// using Microsoft.Maui.ApplicationModel; // IBrowser collision happens here

namespace DiseaseOutbreaksMobile.Services
{
    public class AuthService
    {
        // Используем полное имя, чтобы избежать неоднозначности.
        private readonly OidcClient _oidcClient;

        public AuthService()
        {
            var options = new OidcClientOptions
            {
                // The authority (Identity Server URL)
                Authority = "https://localhost:7147",
                // Client ID for the MAUI app
                ClientId = "maui-client",
                // The RedirectUri must be configured on both the client and the server
                RedirectUri = "myapp://callback", 
                Scope = "openid profile api1 offline_access",
                // Use our custom MAUI browser implementation
                Browser = new MauiBrowser()
            };

            _oidcClient = new OidcClient(options);
        }

        public async Task<LoginResult> LoginAsync()
        {
            var result = await _oidcClient.LoginAsync(new LoginRequest());
            if (!result.IsError)
            {
                // SecureStorage is globally available in MAUI for token storage
                await SecureStorage.SetAsync("access_token", result.AccessToken); 
            }
            return result;
        }
    }

    // FIX: Используем полное имя IdentityModel.OidcClient.Browser.IBrowser 
    // для устранения ошибки CS0104 (неоднозначная ссылка).
    // FIX: Метод OpenAsync изменен обратно на InvokeAsync, так как компилятор требует его (CS0535)
    public class MauiBrowser : IdentityModel.OidcClient.Browser.IBrowser
    {
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            try
            {
                // Use MAUI's WebAuthenticator to initiate the browser flow
                var authResult = await WebAuthenticator.Default.AuthenticateAsync(
                    new Uri(options.StartUrl),
                    new Uri(options.EndUrl));

                // WebAuthenticator returns properties, which must be converted back to a query string
                var query = string.Join("&", authResult.Properties.Select(kvp =>
                    $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));

                return new BrowserResult
                {
                    ResultType = BrowserResultType.Success,
                    Response = options.EndUrl + "?" + query
                };
            }
            catch (TaskCanceledException)
            {
                return new BrowserResult { ResultType = BrowserResultType.UserCancel };
            }
            catch (Exception ex)
            {
                return new BrowserResult { ResultType = BrowserResultType.UnknownError, Error = ex.Message };
            }
        }
    }
}