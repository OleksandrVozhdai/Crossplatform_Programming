using Duende.IdentityServer.Models;

public static class Clients
{
    public static IEnumerable<Client> Get()
    {
        return new[]
        {
            new Client
            {
                ClientId = "maui-client",
                ClientName = "MAUI Mobile App",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,
                RedirectUris = { "myapp://callback" },
                AllowedScopes = { "openid", "profile", "api1" },
                AllowOfflineAccess = true
            }
        };
    }
}