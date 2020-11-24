using AutoMapper;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.User;
using CaffStore.Backend.Interface.Bll.RequestContext;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using CaffStore.Backend.Dal;

namespace CaffStore.Backend.Bll.Services
{
	public class UserService : IUserService
	{
		private readonly CaffStoreDbContext _context;
		private readonly IHttpRequestContext _requestContext;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public UserService(CaffStoreDbContext context, IHttpRequestContext requestContext, UserManager<User> userManager, IMapper mapper)
		{
			_context = context;
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

		public async Task<UserProfileDto> GetMyUserProfileAsync()
		{
			var user = await _userManager.FindByIdAsync(_requestContext.CurrentUserId.ToString());

			ThrowNotFoundIfNull(user);

			var responseUserProfile = _mapper.Map<UserProfileDto>(user);

			responseUserProfile.Roles = _requestContext.CurrentUserRoles;

			return responseUserProfile;
		}

		public async Task<UserProfileDto> UpdateMyUserProfileAsync(UpdateUserProfileDto updateUserProfile)
		{
			var user = await _userManager.FindByIdAsync(_requestContext.CurrentUserId.ToString());

			ThrowNotFoundIfNull(user);

			_mapper.Map(updateUserProfile, user);

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
				throw new CaffStoreBusinessException("Updating user failed", result.Errors.Select(e => e.Description));

			var response = _mapper.Map<UserProfileDto>(user);

			response.Roles = _requestContext.CurrentUserRoles;

			return response;
		}

		public async Task DeleteMyUserProfileAsync()
		{
			var user = await _userManager.FindByIdAsync(_requestContext.CurrentUserId.ToString());

			ThrowNotFoundIfNull(user);

			// Will result in soft delete
			var result = await _userManager.DeleteAsync(user);

			if (!result.Succeeded)
				throw new CaffStoreBusinessException("Deleting user failed", result.Errors.Select(e => e.Description));

			// Delete soft deleted user data
			user.UserName = null;
			user.NormalizedUserName = null;
			user.Email = null;
			user.NormalizedEmail = null;
			user.FirstName = null;
			user.LastName = null;

			await _context.SaveChangesAsync();
		}

		public async Task ChangeMyPasswordAsync(ChangePasswordDto changePassword)
		{
			var user = await _userManager.FindByIdAsync(_requestContext.CurrentUserId.ToString());

			ThrowNotFoundIfNull(user);

			var result = await _userManager.ChangePasswordAsync(user, changePassword.CurrentPassword, changePassword.NewPassword);

			if (!result.Succeeded)
				throw new CaffStoreBusinessException("Change password failed", result.Errors.Select(e => e.Description));
		}

		private void ThrowNotFoundIfNull(User user)
		{
			if (user == null)
				throw new CaffStoreNotFoundException();
		}
	}
}
