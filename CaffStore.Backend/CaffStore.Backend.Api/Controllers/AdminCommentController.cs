using CaffStore.Backend.Api.Identity;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace CaffStore.Backend.Api.Controllers
{
	[Authorize(CaffStorePolicies.AdminOnly)]
	[Route("api/admin/comments")]
	[ApiController]
	public class AdminCommentController : ControllerBase
	{
		private readonly IAdminCommentService _adminCommentService;

		public AdminCommentController(IAdminCommentService adminCommentService)
		{
			_adminCommentService = adminCommentService;
		}

		[HttpDelete("{commentId}",
			Name = nameof(DeleteComment))]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<IActionResult> DeleteComment([FromRoute] long commentId)
		{
			await _adminCommentService.DeleteCommentAsync(commentId);

			return NoContent();
		}
	}
}
