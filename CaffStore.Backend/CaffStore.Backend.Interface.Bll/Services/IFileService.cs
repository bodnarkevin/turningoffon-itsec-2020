using CaffStore.Backend.Interface.Bll.Dtos.File;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface IFileService
	{
		Task<FileDto> UploadFileAsync(IFormFile file);

		Task<FileDto> GetFileAsync(Guid fileId);

		Task DeleteFileAsync(Guid fileId);
	}
}
