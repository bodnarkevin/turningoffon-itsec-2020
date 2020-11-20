using CaffStore.Backend.Interface.Bll.Dtos.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Enums;
using Microsoft.AspNetCore.Http;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface IFileService
	{
		Task<Guid> UploadFileAsync(Stream stream, string extension, FileType fileType);
		Task<FileDto> GetFileAsync(Guid fileId);
		Task DeleteFileAsync(Guid fileId);
		Task SetPreviewFileUri(FileDto file);
		Task SetPreviewFileUris(IEnumerable<FileDto> files);
	}
}
