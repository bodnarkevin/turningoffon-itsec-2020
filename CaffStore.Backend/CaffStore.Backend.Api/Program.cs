using CaffStore.Backend.Api.KeyVault;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CaffStore.Backend.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((context, builder) =>
				{
					if (context.HostingEnvironment.IsProduction())
					{
						builder.AddCostributeAzureKeyVault();
					}
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
