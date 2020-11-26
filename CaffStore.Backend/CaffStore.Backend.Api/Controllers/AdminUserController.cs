using CaffStore.Backend.Api.Identity;
using CaffStore.Backend.Api.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Dtos.Error;
using CaffStore.Backend.Interface.Bll.Dtos.User;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using CaffStore.Backend.Api.Queries;
using CaffStore.Backend.Interface.Bll.Dtos.AdminUser;

namespace CaffStore.Backend.Api.Controllers
{
	[Authorize(CaffStorePolicies.AdminOnly)]
	[Route("api/admin/users")]
	[ApiController]
	public class AdminUserController : ControllerBase
	{
		private readonly IAdminUserService _adminUserService;

		public AdminUserController(IAdminUserService adminUserService)
		{
			_adminUserService = adminUserService;
		}

		[HttpGet(
			Name = nameof(GetPagedUsers))]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(PagedResponse<UserDto>), (int)HttpStatusCode.OK)]
		public Task<PagedResponse<UserDto>> GetPagedUsers([FromQuery] UserPagedQuery userPagedQuery)
		{
			return _adminUserService.GetPagedUsersAsync(userPagedQuery);
		}

		[HttpGet("{userId}",
			Name = nameof(GetUserProfile))]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(UserProfileDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public Task<UserProfileDto> GetUserProfile([FromRoute] long userId)
		{
			return _adminUserService.GetUserProfileAsync(userId);
		}

		[HttpPut("{userId}",
			Name = nameof(UpdateUserProfile))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(UserProfileDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public async Task<UserProfileDto> UpdateUserProfile([FromRoute] long userId, [FromBody] UpdateUserProfileDto updateUserProfile)
		{
			return await _adminUserService.UpdateUserProfileAsync(userId, updateUserProfile);
		}

		[HttpDelete("{userId}",
			Name = nameof(DeleteUserProfile))]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> DeleteUserProfile([FromRoute] long userId)
		{
			await _adminUserService.DeleteUserProfileAsync(userId);

			return NoContent();
		}

		[HttpGet("{userId}/grantAdmin",
			Name = nameof(GrantUserAdminRole))]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(UserProfileDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public Task<UserProfileDto> GrantUserAdminRole([FromRoute] long userId)
		{
			return _adminUserService.GrantUserAdminRoleAsync(userId);
		}

		[HttpPost("{userId}/changePassword",
			Name = nameof(ChangeUserPassword))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public async Task<IActionResult> ChangeUserPassword([FromRoute] long userId, [FromBody] AdminChangePasswordDto adminChangePassword)
		{
			await _adminUserService.ChangeUserPasswordAsync(userId, adminChangePassword);

			return NoContent();
		}
	}
}
