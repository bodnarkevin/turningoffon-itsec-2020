namespace CaffStore.Backend.Dal.Entities
{
	public interface ISoftDeletableEntity
	{
		public bool IsDeleted { get; set; }
	}
}
