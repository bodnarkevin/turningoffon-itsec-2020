using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace CaffStore.Backend.Api.Identity
{
	public static class IdentityServerConfig
	{
		public static IEnumerable<IdentityResource> IdentityResources =>
			new[]
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Email(),
				new IdentityResources.Profile(),
				new IdentityResource(JwtClaimTypes.Role, new[] {JwtClaimTypes.Role}),
			};

		public static IEnumerable<ApiScope> ApiScopes =>
			new[]
			{
				new ApiScope("api")
				{
					UserClaims = new[] {
						JwtClaimTypes.Id,
						JwtClaimTypes.Role,
					}
				}
			};
		public static IEnumerable<Client> Clients =>
			new[]
			{
				new Client
				{
					ClientId = "swagger",
					AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
					RequireClientSecret = false,
					AllowedScopes = {
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						"api"
					},
				},
				new Client
				{
					ClientId = "spa",
					AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
					RequireClientSecret = false,
					AllowOfflineAccess = true,
					AllowedScopes = {
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.OfflineAccess,
						"api"
					},
				},
				new Client
				{
					ClientId = "spa-test",
					AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
					RequireClientSecret = false,
					AllowOfflineAccess = true,
					AccessTokenLifetime = 300, // 5 minutes
					AllowedScopes = {
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.OfflineAccess,
						"api"
					},
				}
			};
	}
}
