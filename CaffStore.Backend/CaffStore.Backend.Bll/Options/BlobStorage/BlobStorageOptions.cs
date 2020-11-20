namespace CaffStore.Backend.Bll.Options.BlobStorage
{
	public class BlobStorageOptions
	{
		public string ConnectionString { get; set; }

		public string CaffContainerName { get; set; }

		public string PreviewContainerName { get; set; }
	}
}
