using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.CaffItem
{
	public class UpdateCaffItemDto
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }
	}
}
