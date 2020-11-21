using AutoMapper;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Bll.Pagination.Extensions;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.User;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CaffStore.Backend.Bll.Services
{
	public class AdminUserService : IAdminUserService
	{
		private readonly UserManager<User> _userManager;
		private readonly IMapper _mapper;

		public AdminUserService(UserManager<User> userManager, IMapper mapper)
		{
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
			var userEntity = await _userManager
				.Users
				.Where(u => u.Id == userId)
				.SingleOrDefaultAsync();

			ThrowNotFoundIfNull(userEntity);

			var response = _mapper.Map<UserProfileDto>(userEntity);

			response.Roles = await _userManager.GetRolesAsync(userEntity);

			return response;
		}

		private void ThrowNotFoundIfNull(User user)
		{
			if (user == null)
				throw new CaffStoreNotFoundException();
		}
	}
}
