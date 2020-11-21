using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.Comment
{
	public class UpdateCommentDto
	{
		[Required]
		public string Text { get; set; }
	}
}
