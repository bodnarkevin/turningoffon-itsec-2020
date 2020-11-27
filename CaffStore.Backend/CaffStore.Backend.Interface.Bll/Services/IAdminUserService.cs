using CaffStore.Backend.Interface.Bll.Dtos.AdminUser;
using CaffStore.Backend.Interface.Bll.Dtos.User;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using CaffStore.Backend.Interface.Bll.Queries;
using System.Threading.Tasks;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface IAdminUserService
	{
		Task<PagedResponse<UserDto>> GetPagedUsersAsync(IUserPagedQuery userPagedQuery);
		Task<UserProfileDto> GetUserProfileAsync(long userId);
		Task<UserProfileDto> UpdateUserProfileAsync(long userId, UpdateUserProfileDto updateUserProfile);
		Task DeleteUserProfileAsync(long userId);
		Task ChangeUserPasswordAsync(long userId, AdminChangePasswordDto changePassword);
		Task<UserProfileDto> GrantUserAdminRoleAsync(long userId);
	}
}
