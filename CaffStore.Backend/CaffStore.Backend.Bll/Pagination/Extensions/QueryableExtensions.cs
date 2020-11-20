using AutoMapper;
using AutoMapper.QueryableExtensions;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CaffStore.Backend.Bll.Pagination.Extensions
{
	public static class QueryableExtensions
	{
		public static async Task<PagedResponse<TDestination>> ToPagedAsync<TSource, TDestination>(this IQueryable<TSource> query, IPagedQuery pagedQuery, IMapper mapper) where TSource : class
		{
			var result = new PagedResponse<TDestination>
			{
				CurrentPage = pagedQuery.Page,
				PageSize = pagedQuery.PageSize,
				TotalResultCount = await query.CountAsync()
			};

			var skip = (pagedQuery.Page - 1) * pagedQuery.PageSize;

			result.Results = await query
				.Skip(skip)
				.Take(pagedQuery.PageSize)
				.ProjectTo<TDestination>(mapper.ConfigurationProvider)
				.ToListAsync();

			return result;
		}
	}
}
