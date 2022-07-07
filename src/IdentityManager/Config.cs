using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityManager;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
      new[]
      {
    new IdentityResources.OpenId(),
    new IdentityResources.Profile(),
    new IdentityResource
    {
      Name = "role",
      UserClaims = new List<string> {"role"}
    }
      };

    public static IEnumerable<ApiScope> ApiScopes =>
      new[]
      {
    new ApiScope("bankapi.customer"),
    new ApiScope("bankapi.employeer"),
    new ApiScope("bankapi.loanadministrator"),
    new ApiScope("bankapi.advisor")
      };
    public static IEnumerable<ApiResource> ApiResources => new[]
    {
  new ApiResource("bankapi")
  {
    Scopes = new List<string> { "bankapi.customer", "bankapi.employeer", "bankapi.loanadministrator", "bankapi.advisor" },
    ApiSecrets = new List<Secret> {new Secret("ScopeSecret".Sha256())},
    UserClaims = new List<string> {"role"}
  }
};

    public static IEnumerable<Client> Clients =>
      new[]
      {
    // m2m client credentials flow client
    new Client
    {
      ClientId = "m2m.client",
      ClientName = "Client Credentials Client",

      AllowedGrantTypes = GrantTypes.ClientCredentials,
      ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

      AllowedScopes = { "bankapi.advisor"}
    },

    // interactive client using code flow + pkce
    new Client
    {
      ClientId = "interactive",
      ClientSecrets = {new Secret("SuperSecretPassword".Sha256())},

      AllowedGrantTypes = GrantTypes.Code,

      RedirectUris = {"https://localhost:5444/signin-oidc", "https://www.getpostman.com/oauth2/callback"},
      FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
      PostLogoutRedirectUris = {"https://localhost:5444/signout-callback-oidc"},

      AllowOfflineAccess = true,
      AllowedScopes = {"openid", "profile", "bankapi.customer"},
      RequirePkce = true,
      RequireConsent = true,
      AllowPlainTextPkce = false
    },
      };
}
