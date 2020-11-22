using System;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Dal.Entities
{
	public class Comment : ISoftDeletableEntity, IAuditableEntity
	{
		public long Id { get; set; }

		[Required]
		public string Text { get; set; }

		public bool IsDeleted { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public long? CreatedById { get; set; }

		public User CreatedBy { get; set; }

		public DateTimeOffset LastModifiedAt { get; set; }

		public long? LastModifiedById { get; set; }

		public User LastModifiedBy { get; set; }
	}
}
