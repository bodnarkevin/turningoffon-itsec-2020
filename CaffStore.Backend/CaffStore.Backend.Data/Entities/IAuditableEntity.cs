using System;

namespace CaffStore.Backend.Dal.Entities
{
	public interface IAuditableEntity
	{
		public DateTimeOffset CreatedAt { get; set; }

		public int? CreatedById { get; set; }

		public User CreatedBy { get; set; }

		public DateTimeOffset LastModifiedAt { get; set; }

		public int? LastModifiedById { get; set; }

		public User LastModifiedBy { get; set; }
	}
}
