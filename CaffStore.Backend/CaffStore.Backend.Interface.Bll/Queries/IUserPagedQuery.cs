using CaffStore.Backend.Interface.Bll.Pagination.Queries;

namespace CaffStore.Backend.Interface.Bll.Queries
{
	public interface IUserPagedQuery : IPagedQuery
	{
		public string Email { get; set; }

		public bool IncludeAdmins { get; set; }
	}
}
