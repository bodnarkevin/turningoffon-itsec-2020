namespace CaffStore.Backend.Dal.Entities
{
	public class CiffDataTag
	{
		public long Id { get; set; }

		public long CiffDataId { get; set; }

		public CiffData CiffData { get; set; }

		public long TagId { get; set; }

		public Tag Tag { get; set; }
	}
}
