using System;

namespace CaffStore.Backend.Bll.Options.BlobStorage
{
	public class BlobStorageOptions
	{
		public Uri StorageAccountUri { get; set; }

		public string StorageAccountName { get; set; }

		public string StorageAccountKey { get; set; }

		public string PreviewContainerName { get; set; }

		public string CaffContainerName { get; set; }

		public int SasTokenLifetimeSeconds { get; set; }
	}
}
