using System.Collections.Generic;
using System.Security.Claims;

namespace CaffStore.Backend.Interface.Bll.RequestContext
{
	public interface IHttpRequestContext
	{
		public ClaimsPrincipal CurrentUser { get; }

		public int? CurrentUserId { get; }

		public IEnumerable<string> CurrentUserRoles { get; }
	}
}
