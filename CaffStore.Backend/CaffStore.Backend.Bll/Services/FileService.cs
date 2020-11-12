using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.File;
using CaffStore.Backend.Interface.Bll.Services;
using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace CaffStore.Backend.Bll.Services
{
	public class FileService : IFileService
	{
		private readonly CaffStoreDbContext _context;

		private readonly BlobContainerClient _blobContainerClient;

		public FileService(CaffStoreDbContext context, BlobContainerClient blobContainerClient)
		{
			_context = context;
			_blobContainerClient = blobContainerClient;
		}

		public async Task<FileDto> UploadFileAsync(UploadFileDto uploadFile)
		{
			var fileEntity = new File
			{
				Id = Guid.NewGuid(),
				Extension = System.IO.Path.GetExtension(uploadFile.File.FileName),
			};

			_context.Files.Add(fileEntity);

			var blobName = fileEntity.Id + fileEntity.Extension;
			var blobClient = _blobContainerClient.GetBlobClient(blobName);

			await blobClient.UploadAsync(uploadFile.File.OpenReadStream());

			await _context.SaveChangesAsync();

			return new FileDto
			{
				Id = fileEntity.Id,
				FileUri = blobClient.Uri,
			};
		}

		public async Task<FileDto> GetFileAsync(Guid fileId)
		{
			var fileEntity = await _context
				.Files
				.FindAsync(fileId);

			if (fileEntity == null)
				throw new CaffStoreNotFoundException("File not found");

			var blobName = fileEntity.Id + fileEntity.Extension;
			var blobClient = _blobContainerClient.GetBlobClient(blobName);

			var exists = await blobClient.ExistsAsync();
			if (!exists)
				throw new CaffStoreNotFoundException("File not found");

			return new FileDto
			{
				Id = fileEntity.Id,
				FileUri = blobClient.Uri,
			};
		}

		public async Task DeleteFileAsync(Guid fileId)
		{
			var fileEntity = await _context
				.Files
				.FindAsync(fileId);

			if (fileEntity == null)
				return;

			var blobName = fileEntity.Id + fileEntity.Extension;
			var blobClient = _blobContainerClient.GetBlobClient(blobName);

			await blobClient.DeleteIfExistsAsync();

			_context.Files.Remove(fileEntity);

			await _context.SaveChangesAsync();
		}
	}
}
