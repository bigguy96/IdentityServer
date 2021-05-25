using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer.Configuration
{
    public class InMemoryConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope>
            {
                new("sampleApi", "Sample API")
            };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new("sampleApi", "Sample API")
                {
                    Scopes = { "sampleApi" }
                }
            };

        public static List<TestUser> GetUsers() =>
            new()
            {
                new TestUser
                {
                    SubjectId = "a9ea0f25-b964-409f-bcce-c923266249b4",
                    Username = "Mick",
                    Password = "MickPassword",
                    Claims = new List<Claim>
                    {
                        new("given_name", "Mick"),
                        new("family_name", "Mining")
                    }
                },
                new TestUser
                {
                    SubjectId = "c95ddb8c-79ec-488a-a485-fe57a1462340",
                    Username = "Jane",
                    Password = "JanePassword",
                    Claims = new List<Claim>
                    {
                        new("given_name", "Jane"),
                        new("family_name", "Downing")
                    }
                }
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new()
                {
                    ClientId = "company-employee",
                    ClientSecrets = new [] { new Secret("codemazesecret".Sha512()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, "sampleApi" }
                },
                new()
                {
                    ClientName = "MVC Client",
                    ClientId = "mvc-client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris = new List<string>{ "https://localhost:5010/signin-oidc" },
                    RequirePkce = false,
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile },
                    ClientSecrets = { new Secret("MVCSecret".Sha512()) },
                    PostLogoutRedirectUris = new List<string> { "https://localhost:5010/signout-callback-oidc" }
                }
            };
    }
}