using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.CaffItem
{
	public class AddCaffItemDto
	{
		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		[Required]
		public IFormFile CaffFile { get; set; }
	}
}
