using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CaffStore.Backend.Api.Pagination.Queries
{
	public class PagedQuery : IPagedQuery
	{
		[FromQuery(Name = "page")]
		[DefaultValue(1)]
		[Range(1, int.MaxValue)]
		public int Page { get; set; } = 1;

		[FromQuery(Name = "pageSize")]
		[DefaultValue(10)]
		[Range(1, 100)]
		public int PageSize { get; set; } = 10;
	}
}
