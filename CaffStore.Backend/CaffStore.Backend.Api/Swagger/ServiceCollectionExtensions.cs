using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CaffStore.Backend.Api.Swagger
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCaffStoreSwaggerGen(this IServiceCollection services)
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

					options.AddSecurityDefinition("caffStoreAuth", new OpenApiSecurityScheme
					{
						Type = SecuritySchemeType.OAuth2,
						Flows = new OpenApiOAuthFlows
						{
							Password = new OpenApiOAuthFlow
							{
								TokenUrl = new Uri("/connect/token", UriKind.Relative),
								Scopes = new Dictionary<string, string>
								{
									{ "api", "Provides access to the API" }
								}
							}
						}
					});

					options.OperationFilter<AuthorizeAttributeOperationFilter>();
				});
		}
	}
}
