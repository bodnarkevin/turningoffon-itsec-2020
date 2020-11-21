using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.Comment
{
	public class AddCommentDto
	{
		[Required]
		public string Text { get; set; }
	}
}
