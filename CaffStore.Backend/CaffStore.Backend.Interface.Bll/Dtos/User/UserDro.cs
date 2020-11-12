using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.User
{
	public class UserDto
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		public string FullName => string.Join(' ', FirstName, LastName);
	}
}
