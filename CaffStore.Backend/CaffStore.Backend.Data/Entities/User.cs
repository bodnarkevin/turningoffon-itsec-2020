using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Dal.Entities
{
	public class User : IdentityUser<int>, ISoftDeletableEntity
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		public bool IsDeleted { get; set; }
	}
}
