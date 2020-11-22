using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.CaffItem
{
	public class CiffDataDto
	{
		public int Width { get; set; }

		public int Height { get; set; }

		[Required]
		public string Caption { get; set; }

		[Required]
		public IEnumerable<string> Tags { get; set; }
	}
}
