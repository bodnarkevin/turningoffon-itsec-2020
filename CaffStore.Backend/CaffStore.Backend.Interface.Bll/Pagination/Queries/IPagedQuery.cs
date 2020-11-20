namespace CaffStore.Backend.Interface.Bll.Pagination.Queries
{
	public interface IPagedQuery
	{
		public int Page { get; set; }

		public int PageSize { get; set; }
	}
}
