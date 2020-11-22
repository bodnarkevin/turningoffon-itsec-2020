﻿using System.ComponentModel.DataAnnotations;
using CaffStore.Backend.Interface.Bll.Dtos.File;
using CaffStore.Backend.Interface.Bll.Dtos.User;

namespace CaffStore.Backend.Interface.Bll.Dtos.CaffItem
{
	public class CaffItemDto
	{
		public long Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		public int DownloadedTimes { get; set; }

		public FileDto PreviewFile { get; set; }

		public UserDto CreatedBy { get; set; }
	}
}
