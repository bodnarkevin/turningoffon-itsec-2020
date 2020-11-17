using AutoMapper;
using CaffStore.Backend.Bll.Pagination.Extensions;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.Caff;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using CaffStore.Backend.Interface.Bll.RequestContext;
using CaffStore.Backend.Interface.Bll.Services;
using System.Threading.Tasks;

namespace CaffStore.Backend.Bll.Services
{
	public class CaffItemService : ICaffItemService
	{
		private readonly CaffStoreDbContext _context;

		private readonly IHttpRequestContext _requestContext;

		private readonly IMapper _mapper;

		public CaffItemService(CaffStoreDbContext context, IHttpRequestContext requestContext, IMapper mapper)
		{
			_context = context;
			_requestContext = requestContext;
			_mapper = mapper;
		}

		public async Task<PagedResult<CaffItemDto>> ListPagedCaffItemsAsync(IPaginationQuery paginationQuery)
		{
			return await _context
				.CaffItems
				.ToPagedAsync<CaffItem, CaffItemDto>(paginationQuery, _mapper);
		}
	}
}
