using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.CaffItem
{
	public class CaffAnimationDataDto
	{
		[Required]
		public int Order { get; set; }

		[Required]
		public int Duration { get; set; }

		[Required]
		public CiffDataDto CiffData { get; set; }
	}
}
