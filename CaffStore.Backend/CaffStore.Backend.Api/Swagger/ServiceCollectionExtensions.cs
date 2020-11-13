using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CaffStore.Backend.Bll.Options.Swagger;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CaffStore.Backend.Api.Swagger
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCaffStoreSwaggerGen(this IServiceCollection services, IConfiguration configuration)
		{
			return services
				.AddSwaggerGen(options =>
				{
					options.SwaggerDoc("v1", new OpenApiInfo { Title = "Caff Store API", Version = "v1" });

					// Remove Dto endings from models
					//options.CustomSchemaIds(type =>
					//{
					//	var name = type.Name;
					//	const string dtoSuffix = "Dto";
					//	return name.EndsWith(dtoSuffix) ? name.Remove(name.Length - dtoSuffix.Length) : name;
					//});

					var swaggerOptions = new SwaggerGenOptions();
					configuration.Bind(nameof(SwaggerGenOptions), swaggerOptions);

					options.AddServer(new OpenApiServer
					{
						Url = swaggerOptions.ServerUri
					});

					options.AddSecurityDefinition("caffStoreAuth", new OpenApiSecurityScheme
					{
						Type = SecuritySchemeType.OAuth2,
						Flows = new OpenApiOAuthFlows
						{
							Password = new OpenApiOAuthFlow
							{
								TokenUrl = new Uri(swaggerOptions.TokenUri),
								//Scopes = new Dictionary<string, string>
								//{
								//	{ "api", "Provides access to the API" },
								//	{ "openid", "" },
								//	{ "profile", "" },
								//	{ "offline_access", "" },
								//}
							}
						}
					});

					options.OperationFilter<AuthorizeAttributeOperationFilter>();
				});
		}
	}
}
