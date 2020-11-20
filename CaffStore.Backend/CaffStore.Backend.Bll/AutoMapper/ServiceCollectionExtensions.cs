using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace CaffStore.Backend.Bll.AutoMapper
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCaffStoreAutoMapper(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(ServiceCollectionExtensions));

			return services;
		}
	}
}
