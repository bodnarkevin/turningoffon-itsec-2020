using System;
using System.ComponentModel.DataAnnotations;
using CaffStore.Backend.Interface.Bll.Dtos.User;

namespace CaffStore.Backend.Interface.Bll.Dtos.Comment
{
	public class CommentDto
	{
		[Required]
		public long Id { get; set; }

		[Required]
		public string Text { get; set; }

		[Required]
		public DateTimeOffset CreatedAt { get; set; }

		// Null if User is deleted
		public UserDto CreatedBy { get; set; }

		[Required]
		public DateTimeOffset LastModifiedAt { get; set; }
	}
}
