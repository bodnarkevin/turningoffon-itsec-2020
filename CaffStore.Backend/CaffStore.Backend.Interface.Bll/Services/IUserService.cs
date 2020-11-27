using CaffStore.Backend.Interface.Bll.Dtos.User;
using System.Threading.Tasks;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface IUserService
	{
		Task RegisterUserAsync(RegisterUserDto registerUser);
		Task<UserProfileDto> GetMyUserProfileAsync();
		Task<UserProfileDto> UpdateMyUserProfileAsync(UpdateUserProfileDto updateUserProfile);
		Task DeleteMyUserProfileAsync();
		Task ChangeMyPasswordAsync(ChangePasswordDto changePassword);
	}
}
