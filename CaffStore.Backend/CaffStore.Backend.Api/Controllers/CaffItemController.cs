using CaffStore.Backend.Api.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Dtos.Caff;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CaffStore.Backend.Api.Controllers
{
	[Authorize]
	[Route("api/caffItems")]
	[ApiController]
	public class CaffItemController : ControllerBase
	{
		private readonly ICaffItemService _caffItemService;

		public CaffItemController(ICaffItemService caffItemService)
		{
			_caffItemService = caffItemService;
		}

		[HttpGet(
			Name = nameof(ListPagedCaffItems))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(PagedResult<CaffItemDto>), (int)HttpStatusCode.OK)]
		public Task<PagedResult<CaffItemDto>> ListPagedCaffItems([FromQuery] PaginationQuery paginationQuery)
		{
			return _caffItemService.ListPagedCaffItemsAsync(paginationQuery);
		}
	}
}
