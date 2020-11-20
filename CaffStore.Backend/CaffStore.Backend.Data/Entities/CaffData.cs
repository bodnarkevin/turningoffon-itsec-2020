using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Dal.Entities
{
	public class CaffData
	{
		public long Id { get; set; }

		[Required]
		public string Creator { get; set; }

		public DateTime Creation { get; set; }

		public ICollection<CaffAnimationData> Animations { get; set; }
	}
}
