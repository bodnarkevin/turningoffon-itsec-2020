using Microsoft.AspNetCore.Identity;

namespace CaffStore.Backend.Dal.Entities
{
	public class User : IdentityUser<long>, ISoftDeletableEntity
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public bool IsDeleted { get; set; }
	}
}
