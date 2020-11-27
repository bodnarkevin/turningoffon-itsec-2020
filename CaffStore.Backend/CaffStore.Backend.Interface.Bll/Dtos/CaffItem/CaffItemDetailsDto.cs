using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.CaffItem
{
	public class CaffItemDetailsDto : CaffItemDto
	{
		[Required]
		public CaffDataDto CaffData { get; set; }
	}
}
