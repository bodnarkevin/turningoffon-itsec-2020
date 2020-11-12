using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace CaffStore.Backend.Api.Identity
{
	public static class IdentityServerConfig
	{
		public static IEnumerable<IdentityResource> IdentityResources =>
			new IdentityResource[]
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Email(),
				new IdentityResources.Profile(),
			};

		public static IEnumerable<ApiScope> ApiScopes =>
			new ApiScope[]
			{
				new ApiScope("api")
				{
					UserClaims = new string[] {
						JwtClaimTypes.Id,
						JwtClaimTypes.Role,
					}
				}
			};
		public static IEnumerable<Client> Clients =>
			new Client[]
			{
				new Client
				{
					ClientId = "swagger",
					AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
					RequireClientSecret = false,
					AllowedScopes = { "openid", "profile", "api" },
				},
				new Client
				{
					ClientId = "spa",
					AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
					RequireClientSecret = false,
					AllowOfflineAccess = true,
					AllowedScopes = { "openid", "profile", "api" },
				}
			};
	}
}
