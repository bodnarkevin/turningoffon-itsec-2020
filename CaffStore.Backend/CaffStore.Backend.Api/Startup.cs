using CaffStore.Backend.Api.Identity;
using CaffStore.Backend.Api.Swagger;
using CaffStore.Backend.Bll.AutoMapper;
using CaffStore.Backend.Bll.Services.Extensions;
using CaffStore.Backend.Dal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace CaffStore.Backend.Api
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers()
				.AddJsonOptions(options =>
				{
					// Enums as strings in json
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
				});

			services.AddDbContext<CaffStoreDbContext>(options =>
				options.UseSqlServer(
					_configuration.GetConnectionString("CaffStoreConnection"),
					builder => builder.MigrationsAssembly("CaffStore.Backend.Api")));

			services.AddCaffStoreAutoMapper();

			services.AddCaffStoreBusinessServices();

			services.AddCaffStoreIdentity();

			services.AddCaffStoreIdentityServer(_configuration);

			services.AddCaffStoreAuthentication(_configuration);

			services.AddCaffStoreSwaggerGen(_configuration);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseExceptionHandler("/api/error");

			app.UseHttpsRedirection();

			// Needed for hosting in Azure Web App
			var forwardOptions = new ForwardedHeadersOptions
			{
				ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
				RequireHeaderSymmetry = false
			};
			forwardOptions.KnownNetworks.Clear();
			forwardOptions.KnownProxies.Clear();
			app.UseForwardedHeaders(forwardOptions);

			app.UseRouting();

			app.UseIdentityServer();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "Caff Store API v1");

				options.OAuthClientId("swagger");
				options.DisplayOperationId();
			});
		}
	}
}
