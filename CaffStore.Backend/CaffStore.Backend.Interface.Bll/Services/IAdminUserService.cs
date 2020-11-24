using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Dtos.AdminUser;
using CaffStore.Backend.Interface.Bll.Dtos.User;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface IAdminUserService
	{
		Task<PagedResponse<UserDto>> GetPagedUsersAsync(IPagedQuery pagedQuery);
		Task<UserProfileDto> GetUserProfileAsync(long userId);
		Task<UserProfileDto> UpdateUserProfileAsync(long userId, UpdateUserProfileDto updateUserProfile);
		Task DeleteUserProfileAsync(long userId);
		Task ChangeUserPasswordAsync(long userId, AdminChangePasswordDto changePassword);
	}
}
