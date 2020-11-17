using System.ComponentModel.DataAnnotations;
using CaffStore.Backend.Interface.Bll.Dtos.User;

namespace CaffStore.Backend.Interface.Bll.Dtos.Caff
{
	public class CaffItemDto
	{
		public long Id { get; set; }

		[Required]
		public string Title { get; set; }

		[Required]
		public string Description { get; set; }

		public int DownloadedTimes { get; set; }

		public UserDto CreatedBy { get; set; }
	}
}
