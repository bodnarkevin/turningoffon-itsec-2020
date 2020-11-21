namespace CaffStore.Backend.Dal.Entities
{
	public class CaffItemComment : ISoftDeletableEntity
	{
		public long Id { get; set; }

		public long CaffItemId { get; set; }

		public CaffItem CaffItem { get; set; }

		public long CommentId { get; set; }

		public Comment Comment { get; set; }

		public bool IsDeleted { get; set; }
	}
}
