using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Interface.Bll.Pagination.Responses
{
	public class PagedResult<TResult>
	{
		public int CurrentPage { get; set; }

		public int PageSize { get; set; }

		public int TotalResultCount { get; set; }

		public int TotalPageCount => ((TotalResultCount - 1) / PageSize) + 1;

		[Required]
		public ICollection<TResult> Results { get; set; }
	}
}
