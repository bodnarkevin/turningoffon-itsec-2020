using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.User
{
	public class UpdateUserProfileDto
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }
	}
}
