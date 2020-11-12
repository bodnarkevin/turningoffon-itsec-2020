using System;
using Azure.Storage.Blobs;
using CaffStore.Backend.Bll.Options.BlobStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CaffStore.Backend.Api.BlobStorage
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddCaffStoreBlobStorage(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped(implementationFactory =>
			{
				var blobStorageOptions = new BlobStorageOptions();
				configuration.Bind(nameof(BlobStorageOptions), blobStorageOptions);

				var blobServiceClient = new BlobServiceClient(blobStorageOptions.ConnectionString);

				var blobContainerClient = blobServiceClient.GetBlobContainerClient(blobStorageOptions.ContainerName);

				return blobContainerClient;
			});

			return services;
		}
	}
}
