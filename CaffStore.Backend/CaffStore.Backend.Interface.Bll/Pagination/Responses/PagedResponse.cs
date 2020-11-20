using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Pagination.Responses
{
	public class PagedResponse<TResult>
	{
		[Required]
		public int CurrentPage { get; set; }

		[Required]
		public int PageSize { get; set; }

		[Required]
		public int TotalResultCount { get; set; }

		[Required]
		public int TotalPageCount => ((TotalResultCount - 1) / PageSize) + 1;

		[Required]
		public ICollection<TResult> Results { get; set; }
	}
}
