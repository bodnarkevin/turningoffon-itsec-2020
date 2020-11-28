using System.Collections.Generic;
using System.Security.Claims;

namespace CaffStore.Backend.Interface.Bll.RequestContext
{
	public interface IHttpRequestContext
	{
		public ClaimsPrincipal CurrentUser { get; }

		public long? CurrentUserId { get; }

		public IEnumerable<string> CurrentUserRoles { get; }
	}
}
