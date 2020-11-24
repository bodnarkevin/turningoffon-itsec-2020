using CaffStore.Backend.Api.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;
using CaffStore.Backend.Interface.Bll.Dtos.Error;
using CaffStore.Backend.Interface.Bll.Dtos.File;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
			Name = nameof(GetPagedCaffItems))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(PagedResponse<CaffItemDto>), (int)HttpStatusCode.OK)]
		public Task<PagedResponse<CaffItemDto>> GetPagedCaffItems([FromQuery] PagedQuery pagedQuery)
		{
			return _caffItemService.GetPagedCaffItemsAsync(pagedQuery);
		}

		[HttpPost(
			Name = nameof(AddCaffItem))]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(CaffItemDetailsDto), (int)HttpStatusCode.Created)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
		public async Task<ActionResult<CaffItemDetailsDto>> AddCaffItem([FromForm] AddCaffItemDto addCaffItem)
		{
			var response = await _caffItemService.AddCaffItemAsync(addCaffItem);

			return CreatedAtAction(nameof(GetCaffItem), new {caffItemId = response.Id}, response);
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

		[HttpPut("{caffItemId}",
			Name = nameof(UpdateMyCaffItem))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(CaffItemDetailsDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public Task<CaffItemDetailsDto> UpdateMyCaffItem([FromRoute] long caffItemId,
			[FromBody] UpdateCaffItemDto updateCaffItem)
		{
			return _caffItemService.UpdateMyCaffItemAsync(caffItemId, updateCaffItem);
		}

		[HttpDelete("{caffItemId}",
			Name = nameof(DeleteMyCaffItem))]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<IActionResult> DeleteMyCaffItem([FromRoute] long caffItemId)
		{
			await _caffItemService.DeleteMyCaffItemAsync(caffItemId);

			return NoContent();
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

		[HttpGet("{caffItemId}/comments",
			Name = nameof(GetCaffItemComments))]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(IEnumerable<CommentDto>), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public Task<IEnumerable<CommentDto>> GetCaffItemComments([FromRoute] long caffItemId)
		{
			return _caffItemService.GetCaffItemCommentsAsync(caffItemId);
		}

		[HttpPost("{caffItemId}/comments",
			Name = nameof(AddCaffItemComment))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(CommentDto), (int)HttpStatusCode.Created)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public async Task<ActionResult<CommentDto>> AddCaffItemComment([FromRoute] long caffItemId, [FromBody] AddCommentDto addComment)
		{
			var response = await _caffItemService.AddCaffItemCommentAsync(caffItemId, addComment);
			
			return CreatedAtAction(nameof(CommentController.GetComment), "Comment",new {commentId = response.Id}, response);
		}
	}
}
