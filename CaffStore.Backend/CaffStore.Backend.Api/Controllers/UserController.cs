using System.Net;
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
			Name = nameof(GetUserProfile))]
		[ProducesResponseType(typeof(UserProfileDto), (int)HttpStatusCode.OK)]
		public async Task<UserProfileDto> GetUserProfile()
		{
			return await _userService.GetUserProfileAsync();
		}

		[Authorize]
		[HttpPost("me",
			Name = nameof(UpdateUserProfile))]
		[ProducesResponseType(typeof(UserProfileDto), (int)HttpStatusCode.OK)]
		public async Task<UserProfileDto> UpdateUserProfile([FromBody] UpdateUserProfileDto updateUserProfile)
		{
			return await _userService.UpdateUserProfileAsync(updateUserProfile);
		}

		[Authorize]
		[HttpPost("me/changePassword",
			Name = nameof(ChangePassword))]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
		public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePassword)
		{
			await _userService.ChangePasswordAsync(changePassword);

			return NoContent();
		}
	}
}
