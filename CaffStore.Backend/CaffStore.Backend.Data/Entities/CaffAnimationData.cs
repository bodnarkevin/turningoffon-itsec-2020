namespace CaffStore.Backend.Dal.Entities
{
	public class CaffAnimationData
	{
		public long Id { get; set; }

		public int Order { get; set; }

		public int Duration { get; set; }

		public long CiffDataId { get; set; }

		public CiffData CiffData { get; set; }
	}
}
