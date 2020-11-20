using System;

namespace CaffStore.Backend.Dal.Entities
{
	public interface IAuditableEntity
	{
		public DateTimeOffset CreatedAt { get; set; }

		public long? CreatedById { get; set; }

		public User CreatedBy { get; set; }

		public DateTimeOffset LastModifiedAt { get; set; }

		public long? LastModifiedById { get; set; }

		public User LastModifiedBy { get; set; }
	}
}
