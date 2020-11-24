using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.AdminUser
{
	public class AdminChangePasswordDto
	{
		[Required]
		public string NewPassword { get; set; }
	}
}
