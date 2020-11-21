using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Bll.Options.BlobStorage;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.File;
using CaffStore.Backend.Interface.Bll.Enums;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File = CaffStore.Backend.Dal.Entities.File;

namespace CaffStore.Backend.Bll.Services
{
	public class FileService : IFileService
	{
		private readonly CaffStoreDbContext _context;
		private readonly StorageSharedKeyCredential _sharedKeyCredential;
		private readonly BlobContainerClient _caffBlobContainerClient;
		private readonly BlobContainerClient _previewBlobContainerClient;
		private readonly int _sasTokenLifetimeSeconds;

		public FileService(CaffStoreDbContext context, IConfiguration configuration)
		{
			_context = context;

			var blobStorageOptions = new BlobStorageOptions();
			configuration.Bind(nameof(BlobStorageOptions), blobStorageOptions);

			_sasTokenLifetimeSeconds = blobStorageOptions.SasTokenLifetimeSeconds;

			_sharedKeyCredential = new StorageSharedKeyCredential(blobStorageOptions.StorageAccountName, blobStorageOptions.StorageAccountKey);

			var blobServiceClient = new BlobServiceClient(blobStorageOptions.StorageAccountUri, _sharedKeyCredential);

			_caffBlobContainerClient = blobServiceClient.GetBlobContainerClient(blobStorageOptions.CaffContainerName);
			_previewBlobContainerClient = blobServiceClient.GetBlobContainerClient(blobStorageOptions.PreviewContainerName);
		}

		public async Task<Guid> UploadFileAsync(Stream stream, string extension, FileType fileType)
		{
			File fileEntity = fileType switch
			{
				FileType.Caff => new CaffFile(),
				FileType.Preview => new PreviewFile(),
				_ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
			};

			fileEntity.Id = Guid.NewGuid();
			fileEntity.Extension = extension;

			_context.Files.Add(fileEntity);

			var blobContainerClient = fileType switch
			{
				FileType.Caff => _caffBlobContainerClient,
				FileType.Preview => _previewBlobContainerClient,
				_ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
			};

			var blobName = fileEntity.Id + fileEntity.Extension;
			var blobClient = blobContainerClient.GetBlobClient(blobName);

			await blobClient.UploadAsync(stream);

			await _context.SaveChangesAsync();

			return fileEntity.Id;
		}

		public async Task<FileDto> GetFileAsync(Guid fileId)
		{
			var fileEntity = await _context
				.Files
				.FindAsync(fileId);

			if (fileEntity == null)
				throw new CaffStoreNotFoundException("File not found");

			var blobName = GetBlobName(fileEntity);

			var blobContainerClient = fileEntity switch
			{
				CaffFile _ => _caffBlobContainerClient,
				PreviewFile _ => _previewBlobContainerClient,
				_ => throw new InvalidOperationException()
			};

			var blobClient = blobContainerClient.GetBlobClient(blobName);

			var uri = fileEntity switch
			{
				CaffFile _ => GenerateSasUri(blobClient.Uri),
				PreviewFile _ => blobClient.Uri,
				_ => throw new InvalidOperationException()
			};

			return new FileDto
			{
				Id = fileEntity.Id,
				FileUri = uri
			};
		}

		public async Task SetPreviewFileUri(FileDto file)
		{
			var fileEntity = await _context
				.Files
				.FindAsync(file.Id);

			if (fileEntity == null)
				return;

			var blobName = GetBlobName(fileEntity);
			var blobClient = _previewBlobContainerClient.GetBlobClient(blobName);

			file.FileUri = blobClient.Uri;
		}

		public async Task SetPreviewFileUris(IEnumerable<FileDto> files)
		{
			var fileDtos = files.ToList();
			var fileIds = fileDtos.Select(f => f.Id);

			// Prefetch all needed files
			await _context
				.Files
				.Where(f => fileIds.Contains(f.Id))
				.ToListAsync();

			foreach (var file in fileDtos)
				await SetPreviewFileUri(file);
		}

		public async Task DeleteFileAsync(Guid fileId)
		{
			var fileEntity = await _context
				.Files
				.FindAsync(fileId);

			if (fileEntity == null)
				return;

			var blobContainerClient = fileEntity switch
			{
				CaffFile _ => _caffBlobContainerClient,
				PreviewFile _ => _previewBlobContainerClient,
				_ => throw new InvalidOperationException()
			};

			var blobName = GetBlobName(fileEntity);
			var blobClient = blobContainerClient.GetBlobClient(blobName);

			await blobClient.DeleteIfExistsAsync();

			_context.Files.Remove(fileEntity);

			await _context.SaveChangesAsync();
		}

		private string GetBlobName(File file)
		{
			return file.Id + file.Extension;
		}

		private Uri GenerateSasUri(Uri bloUri)
		{
			var blobClient = new BlobClient(bloUri, _sharedKeyCredential);
			return blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.Now.AddSeconds(_sasTokenLifetimeSeconds));
		}
	}
}
