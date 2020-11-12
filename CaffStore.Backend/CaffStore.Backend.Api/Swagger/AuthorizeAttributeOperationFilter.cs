using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace CaffStore.Backend.Api.Swagger
{
	public class AuthorizeAttributeOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var hasAuthorize =
				context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
				|| context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

			if (!hasAuthorize)
				return;

			operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
			operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

			operation.Security = new List<OpenApiSecurityRequirement>
			{
				new OpenApiSecurityRequirement
				{
					[
						new OpenApiSecurityScheme {Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "caffStoreAuth"}
						}
					] = new[] {"api"}
				}
			};
		}
	}
}
