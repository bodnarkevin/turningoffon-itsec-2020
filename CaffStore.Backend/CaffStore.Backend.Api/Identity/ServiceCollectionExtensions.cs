using CaffStore.Backend.Bll.Options.IdentityServer;
using CaffStore.Backend.Bll.Options.JwtBearerToken;
using CaffStore.Backend.Common.Helpers;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CaffStore.Backend.Api.Identity
{
	public static class ServiceCollectionExtensions
	{
		public static IdentityBuilder AddCaffStoreIdentity(this IServiceCollection services)
		{
			return services
				.AddIdentityCore<User>(options =>
				{
					options.Password.RequireDigit = true;
					options.Password.RequireLowercase = true;
					options.Password.RequireNonAlphanumeric = false;
					options.Password.RequireUppercase = true;
					options.Password.RequiredLength = 8;
					options.Password.RequiredUniqueChars = 1;
				})
				.AddEntityFrameworkStores<CaffStoreDbContext>()
				.AddDefaultTokenProviders()
				.AddRoles<Role>()
				.AddUserStore<UserStore<User, Role, CaffStoreDbContext, long>>()
				.AddRoleStore<RoleStore<Role, CaffStoreDbContext, long>>()
				.AddSignInManager<SignInManager<User>>();
		}

		public static IIdentityServerBuilder AddCaffStoreIdentityServer(this IServiceCollection services, IConfiguration configuration)
		{
			var builder = services.AddIdentityServer(options =>
			{
				options.Events.RaiseErrorEvents = true;
				options.Events.RaiseInformationEvents = true;
				options.Events.RaiseFailureEvents = true;
				options.Events.RaiseSuccessEvents = true;

				// see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
				//options.EmitStaticAudienceClaim = true;
			})
				.AddInMemoryIdentityResources(IdentityServerConfig.IdentityResources)
				.AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
				.AddInMemoryClients(IdentityServerConfig.Clients)
				.AddAspNetIdentity<User>();

			var identityServerOptions = new IdentityServerOptions();
			configuration.Bind(nameof(IdentityServerOptions), identityServerOptions);

			return identityServerOptions.SigningCredentialSourceType switch
			{
				SigningCredentialSourceType.Developer =>
					builder.AddDeveloperSigningCredential(),

				SigningCredentialSourceType.KeyVault =>
					builder.AddSigningCredential(
						X509Certificate2Helper.GetCertificateFromAzureKeyVaultAsync(
							identityServerOptions.AzureKeyVaultUri, identityServerOptions.AzureKeyVaultSigningCertificateName)),

				_ => builder
			};
		}

		public static AuthenticationBuilder AddCaffStoreAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var jwtBearerTokenOptions = new JwtBearerTokenOptions();
			configuration.Bind(nameof(JwtBearerTokenOptions), jwtBearerTokenOptions);

			return services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					// base-address of your identityserver
					options.Authority = jwtBearerTokenOptions.Authority;

					options.RequireHttpsMetadata = jwtBearerTokenOptions.RequireHttpsMetadata;

					options.TokenValidationParameters.ValidateAudience = false;

					// IdentityServer emits a typ header by default, recommended extra check
					options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
				});
		}

		public static IServiceCollection AddCaffStoreAuthorization(this IServiceCollection services)
		{
			return services
				.AddAuthorizationCore(options =>
				{
					options.AddPolicy(CaffStorePolicies.AdminOnly, policy =>
						policy.RequireRole(CaffStoreRoles.Admin));
				});
		}
	}
}
