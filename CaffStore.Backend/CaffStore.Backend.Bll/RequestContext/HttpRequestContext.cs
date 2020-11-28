using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using CaffStore.Backend.Interface.Bll.RequestContext;

namespace CaffStore.Backend.Bll.RequestContext
{
	public class HttpRequestContext : IHttpRequestContext
	{
		public ClaimsPrincipal CurrentUser { get; }

		public long? CurrentUserId { get; }

		public IEnumerable<string> CurrentUserRoles { get; }

		public HttpRequestContext(IHttpContextAccessor httpContextAccessor)
		{
			CurrentUser = httpContextAccessor.HttpContext?.User;

			if (CurrentUser == null)
				return;

			var userId = CurrentUser.FindFirstValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(userId))
				return;

			CurrentUserId = int.Parse(userId);

			CurrentUserRoles = CurrentUser.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
		}
	}
}
