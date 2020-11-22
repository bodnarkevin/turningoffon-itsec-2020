using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Dal.Entities
{
	public class Tag
	{
		public long Id { get; set; }

		[Required]
		public string Text { get; set; }

		public ICollection<CiffDataTag> CiffData { get; set; }
	}
}
