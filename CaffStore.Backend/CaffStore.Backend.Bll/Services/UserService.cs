using AutoMapper;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.User;
using CaffStore.Backend.Interface.Bll.RequestContext;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace CaffStore.Backend.Bll.Services
{
	public class UserService : IUserService
	{
		private readonly IHttpRequestContext _requestContext;

		private readonly UserManager<User> _userManager;

		private readonly IMapper _mapper;

		public UserService(IHttpRequestContext requestContext, UserManager<User> userManager, IMapper mapper)
		{
			_requestContext = requestContext;
			_userManager = userManager;
			_mapper = mapper;
		}

		public async Task RegisterUserAsync(RegisterUserDto registerUser)
		{
			var user = await _userManager.FindByNameAsync(registerUser.Email);
			if (user != null)
				throw new CaffStoreConflictException("User already registered");

			user = new User
			{
				UserName = registerUser.Email,
				Email = registerUser.Email,
				FirstName = registerUser.FirstName,
				LastName = registerUser.LastName,
			};

			var result = await _userManager.CreateAsync(user, registerUser.Password);

			if (!result.Succeeded)
				throw new CaffStoreBusinessException("Registration failed", result.Errors.Select(e => e.Description));
		}

		public async Task<UserProfileDto> GetUserProfileAsync()
		{
			var user = await _userManager.FindByIdAsync(_requestContext.CurrentUserId.ToString());

			var responseUserProfile = _mapper.Map<UserProfileDto>(user);

			responseUserProfile.Roles = _requestContext.CurrentUserRoles;

			return responseUserProfile;
		}

		public async Task<UserProfileDto> UpdateUserProfileAsync(UpdateUserProfileDto updateUserProfile)
		{
			var user = await _userManager.FindByIdAsync(_requestContext.CurrentUserId.ToString());

			_mapper.Map(updateUserProfile, user);

			await _userManager.UpdateAsync(user);

			var responseUserProfile = _mapper.Map<UserProfileDto>(user);

			responseUserProfile.Roles = _requestContext.CurrentUserRoles;

			return responseUserProfile;
		}

		public async Task ChangePasswordAsync(ChangePasswordDto changePassword)
		{
			var user = await _userManager.FindByIdAsync(_requestContext.CurrentUserId.ToString());

			var result = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);

			if (!result.Succeeded)
				throw new CaffStoreBusinessException("Change password failed", result.Errors.Select(e => e.Description));
		}
	}
}
