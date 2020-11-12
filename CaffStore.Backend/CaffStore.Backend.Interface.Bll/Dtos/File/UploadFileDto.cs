using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.File
{
	public class UploadFileDto
	{
		[Required]
		public IFormFile File { get; set; }
	}
}
