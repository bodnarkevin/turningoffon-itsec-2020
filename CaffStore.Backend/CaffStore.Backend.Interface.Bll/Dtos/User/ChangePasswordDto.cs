using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.User
{
	public class ChangePasswordDto
	{
		[Required]
		public string CurrentPassword { get; set; }

		[Required]
		public string NewPassword { get; set; }
	}
}
