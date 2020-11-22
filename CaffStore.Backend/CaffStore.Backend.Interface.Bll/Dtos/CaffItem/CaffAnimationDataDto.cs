using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.CaffItem
{
	public class CaffAnimationDataDto
	{
		public int Order { get; set; }

		public int Duration { get; set; }

		[Required]
		public CiffDataDto CiffData { get; set; }
	}
}
