using System;
using System.ComponentModel.DataAnnotations;
using CaffStore.Backend.Interface.Bll.Dtos.File;
using CaffStore.Backend.Interface.Bll.Dtos.User;

namespace CaffStore.Backend.Interface.Bll.Dtos.CaffItem
{
	public class CaffItemDto
	{
		[Required]
		public long Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public int DownloadedTimes { get; set; }

		[Required]
		public FileDto PreviewFile { get; set; }

		[Required]
		public DateTimeOffset CreatedAt { get; set; }

		// Null if User is deleted
		public UserDto CreatedBy { get; set; }

		[Required]
		public DateTimeOffset LastModifiedAt { get; set; }
	}
}
