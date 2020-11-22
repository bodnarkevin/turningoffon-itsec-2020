using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Dal.Entities
{
	public class CaffItem : ISoftDeletableEntity, IAuditableEntity
	{
		public long Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		public int DownloadedTimes { get; set; }

		public long CaffDataId { get; set; }

		public CaffData CaffData { get; set; }

		public Guid? CaffFileId { get; set; }

		public CaffFile CaffFile { get; set; }

		public Guid? PreviewFileId { get; set; }

		public PreviewFile PreviewFile { get; set; }

		public ICollection<CaffItemComment> Comments { get; set; }

		public bool IsDeleted { get; set; }

		public DateTimeOffset CreatedAt { get; set; }

		public long? CreatedById { get; set; }

		public User CreatedBy { get; set; }

		public DateTimeOffset LastModifiedAt { get; set; }

		public long? LastModifiedById { get; set; }

		public User LastModifiedBy { get; set; }
	}
}
