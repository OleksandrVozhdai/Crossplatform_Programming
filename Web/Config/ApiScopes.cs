using Duende.IdentityServer.Models;

public static class ApiScopes
{
    public static IEnumerable<ApiScope> Get()
    {
        return new[] { new ApiScope("api1", "My API") };
    }
}