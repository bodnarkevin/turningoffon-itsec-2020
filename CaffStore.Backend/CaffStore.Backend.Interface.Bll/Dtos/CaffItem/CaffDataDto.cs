using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.CaffItem
{
	public class CaffDataDto
	{
		[Required]
		public string Creator { get; set; }

		[Required]
		public DateTime Creation { get; set; }

		[Required]
		public IEnumerable<CaffAnimationDataDto> Animations { get; set; }
	}
}
