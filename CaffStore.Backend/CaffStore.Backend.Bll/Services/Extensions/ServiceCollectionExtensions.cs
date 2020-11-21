using CaffStore.Backend.Bll.RequestContext;
using CaffStore.Backend.Interface.Bll.RequestContext;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CaffStore.Backend.Bll.Services.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCaffStoreBusinessServices(this IServiceCollection services)
		{
			services.AddSingleton<ITimeService, TimeService>();

			services.AddScoped<IHttpRequestContext, HttpRequestContext>();

			services.AddScoped<IFileService, FileService>();

			// Adding business services
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ICaffItemService, CaffItemService>();
			services.AddScoped<ICommentService, CommentService>();

			return services;
		}
	}
}
