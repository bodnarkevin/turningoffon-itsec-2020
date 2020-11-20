using CaffStore.Backend.Api.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Dtos.Caff;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Dtos.Error;
using CaffStore.Backend.Interface.Bll.Dtos.File;

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
			Name = nameof(GetPagedCaffItems))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(PagedResponse<CaffItemDto>), (int)HttpStatusCode.OK)]
		public Task<PagedResponse<CaffItemDto>> GetPagedCaffItems([FromQuery] PagedQuery pagedQuery)
		{
			return _caffItemService.GetPagedCaffItemsAsync(pagedQuery);
		}

		[HttpGet("{caffItemId}",
			Name = nameof(GetCaffItem))]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(CaffItemDetailsDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public Task<CaffItemDetailsDto> GetCaffItem([FromRoute] long caffItemId)
		{
			return _caffItemService.GetCaffItemAsync(caffItemId);
		}

		[HttpPost(
			Name = nameof(AddCaffItem))]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(CaffItemDetailsDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
		public Task<CaffItemDetailsDto> AddCaffItem([FromForm] AddCaffItemDto addCaffItem)
		{
			return _caffItemService.AddCaffItemAsync(addCaffItem);
		}

		[HttpGet("{caffItemId}/download",
			Name = nameof(DownloadCaffItem))]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(FileDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public Task<FileDto> DownloadCaffItem([FromRoute] long caffItemId)
		{
			return _caffItemService.DownloadCaffFileAsync(caffItemId);
		}
	}
}
