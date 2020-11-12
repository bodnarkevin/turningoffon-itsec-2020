using CaffStore.Backend.Interface.Bll.Dtos.User;
using System.Threading.Tasks;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface IUserService
	{
		Task RegisterUserAsync(RegisterUserDto registerUser);
		Task<UserProfileDto> GetUserProfileAsync();
		Task<UserProfileDto> UpdateUserProfileAsync(UpdateUserProfileDto updateUserProfile);
		Task ChangePasswordAsync(ChangePasswordDto changePassword);
	}
}
