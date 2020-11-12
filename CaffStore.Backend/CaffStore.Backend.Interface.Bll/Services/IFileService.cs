using CaffStore.Backend.Interface.Bll.Dtos.File;
using System;
using System.Threading.Tasks;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface IFileService
	{
		Task<FileDto> UploadFileAsync(UploadFileDto uploadFile);
		Task<FileDto> GetFileAsync(Guid fileId);
		Task DeleteFileAsync(Guid fileId);
	}
}
