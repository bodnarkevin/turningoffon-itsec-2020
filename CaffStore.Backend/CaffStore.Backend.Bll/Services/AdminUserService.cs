using AutoMapper;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Bll.Pagination.Extensions;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.AdminUser;
using CaffStore.Backend.Interface.Bll.Dtos.User;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace CaffStore.Backend.Bll.Services
{
	public class AdminUserService : IAdminUserService
	{
		private readonly CaffStoreDbContext _context;
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public AdminUserService(CaffStoreDbContext context, UserManager<User> userManager, IMapper mapper)
		{
			_context = context;
			_userManager = userManager;
			_mapper = mapper;
		}

		public async Task<PagedResponse<UserDto>> GetPagedUsersAsync(IPagedQuery pagedQuery)
		{
			return await _userManager
				.Users
				.ToPagedAsync<User, UserDto>(pagedQuery, _mapper);
		}

		public async Task<UserProfileDto> GetUserProfileAsync(long userId)
		{
			var userEntity = await _userManager.FindByIdAsync(userId.ToString());

			ThrowNotFoundIfNull(userEntity);

			var response = _mapper.Map<UserProfileDto>(userEntity);

			response.Roles = await _userManager.GetRolesAsync(userEntity);

			return response;
		}

		public async Task<UserProfileDto> UpdateUserProfileAsync(long userId, UpdateUserProfileDto updateUserProfile)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());

			ThrowNotFoundIfNull(user);

			_mapper.Map(updateUserProfile, user);

			var result = await _userManager.UpdateAsync(user);

			if (!result.Succeeded)
				throw new CaffStoreBusinessException("Updating user failed", result.Errors.Select(e => e.Description));

			return await GetUserProfileAsync(userId);
		}

		public async Task DeleteUserProfileAsync(long userId)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());

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

		public async Task ChangeUserPasswordAsync(long userId, AdminChangePasswordDto changePassword)
		{
			var user = await _userManager.FindByIdAsync(userId.ToString());

			ThrowNotFoundIfNull(user);

			var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

			var result = await _userManager.ResetPasswordAsync(user, resetToken, changePassword.NewPassword);

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
