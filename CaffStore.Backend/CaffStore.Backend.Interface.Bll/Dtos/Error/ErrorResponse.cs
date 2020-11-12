using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Dtos.Error
{
	public class ErrorResponse
	{
		[Required]
		public int StatusCode { get; set; }

		[Required]
		public string Type { get; set; }

		[Required]
		public string Message { get; set; }

		[Required]
		public IEnumerable<string> Errors { get; set; }

		public string StackTrace { get; set; }
	}
}
