using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Dtos.User;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface IAdminUserService
	{
		Task<PagedResponse<UserDto>> GetPagedUsersAsync(IPagedQuery pagedQuery);
		Task<UserProfileDto> GetUserProfileAsync(long userId);
	}
}
