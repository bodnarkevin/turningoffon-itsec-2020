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
		public static async Task<PagedResult<TDestination>> ToPagedAsync<TSource, TDestination>(this IQueryable<TSource> query, IPaginationQuery paginationQuery, IMapper mapper) where TSource : class
		{
			var result = new PagedResult<TDestination>
			{
				CurrentPage = paginationQuery.Page,
				PageSize = paginationQuery.PageSize,
				TotalResultCount = await query.CountAsync()
			};

			var skip = (paginationQuery.Page - 1) * paginationQuery.PageSize;

			result.Results = await query
				.Skip(skip)
				.Take(paginationQuery.PageSize)
				.ProjectTo<TDestination>(mapper.ConfigurationProvider)
				.ToListAsync();

			return result;
		}
	}
}
