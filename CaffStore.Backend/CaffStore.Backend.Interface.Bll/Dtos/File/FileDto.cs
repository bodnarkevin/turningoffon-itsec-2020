using System;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.File
{
	public class FileDto
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		public Uri FileUri { get; set; }
	}
}
