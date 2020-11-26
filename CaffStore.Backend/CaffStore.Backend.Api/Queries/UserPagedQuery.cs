using CaffStore.Backend.Api.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Queries;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace CaffStore.Backend.Api.Queries
{
	public class UserPagedQuery : PagedQuery, IUserPagedQuery
	{
		[FromQuery(Name = "email")]
		public string Email { get; set; }

		[FromQuery(Name = "includeAdmins")]
		[DefaultValue(false)]
		public bool IncludeAdmins { get; set; } = false;
	}
}
