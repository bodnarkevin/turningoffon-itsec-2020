﻿using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;
using CaffStore.Backend.Interface.Bll.Dtos.Error;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaffStore.Backend.Api.Controllers
{
	[Authorize]
	[Route("api/comments")]
	[ApiController]
	public class CommentController : ControllerBase
	{
		private readonly ICommentService _commentService;

		public CommentController(ICommentService commentService)
		{
			_commentService = commentService;
		}

		[HttpGet("{commentId}",
			Name = nameof(GetComment))]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(CommentDto), (int) HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int) HttpStatusCode.NotFound)]
		public Task<CommentDto> GetComment([FromRoute] long commentId)
		{
			return _commentService.GetCommentAsync(commentId);
		}

		[HttpPut("{commentId}",
			Name = nameof(UpdateComment))]
		[Produces(MediaTypeNames.Application.Json)]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(CommentDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public Task<CommentDto> UpdateComment([FromRoute] long commentId, [FromBody] UpdateCommentDto updateComment)
		{
			return _commentService.UpdateCommentAsync(commentId, updateComment);
		}

		[HttpDelete("{commentId}",
			Name = nameof(DeleteComment))]
		[Produces(MediaTypeNames.Application.Json)]
		[Consumes(MediaTypeNames.Application.Json)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<IActionResult> DeleteComment([FromRoute] long commentId)
		{
			await _commentService.DeleteCommentAsync(commentId);

			return NoContent();
		}
	}
}