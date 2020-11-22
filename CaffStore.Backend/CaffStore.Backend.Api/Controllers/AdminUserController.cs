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

namespace CaffStore.Backend.Api.Controllers
{
	[Authorize(CaffStorePolicies.AdminOnly)]
	[Route("api/admin/users")]
	[ApiController]
	public class AdminUserController
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
		public Task<PagedResponse<UserDto>> GetPagedUsers([FromQuery] PagedQuery pagedQuery)
		{
			return _adminUserService.GetPagedUsersAsync(pagedQuery);
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
	}
}
