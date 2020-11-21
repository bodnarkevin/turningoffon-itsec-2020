using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Dtos.Error;
using CaffStore.Backend.Interface.Bll.Dtos.User;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Authorization;

namespace CaffStore.Backend.Api.Controllers
{
	[Route("api/users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("register",
			Name = nameof(RegisterUser))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.Conflict)]
		public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUser)
		{
			await _userService.RegisterUserAsync(registerUser);
			return NoContent();
		}

		[Authorize]
		[HttpGet("me",
			Name = nameof(GetMyUserProfile))]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(UserProfileDto), (int)HttpStatusCode.OK)]
		public async Task<UserProfileDto> GetMyUserProfile()
		{
			return await _userService.GetMyUserProfileAsync();
		}

		[Authorize]
		[HttpPut("me",
			Name = nameof(UpdateMyUserProfile))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(UserProfileDto), (int)HttpStatusCode.OK)]
		public async Task<UserProfileDto> UpdateMyUserProfile([FromBody] UpdateUserProfileDto updateUserProfile)
		{
			return await _userService.UpdateMyUserProfileAsync(updateUserProfile);
		}

		[Authorize]
		[HttpPost("me/changePassword",
			Name = nameof(ChangeMyPassword))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> ChangeMyPassword([FromBody] ChangePasswordDto changePassword)
		{
			await _userService.ChangeMyPasswordAsync(changePassword);

			return NoContent();
		}
	}
}
