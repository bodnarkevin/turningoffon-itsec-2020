using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Dal.Entities
{
	public class CiffData
	{
		public long Id { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		[Required]
		public string Caption { get; set; }

		public CaffAnimationData CaffAnimationData { get; set; }

		public ICollection<CiffDataTag> Tags { get; set; }
	}
}
