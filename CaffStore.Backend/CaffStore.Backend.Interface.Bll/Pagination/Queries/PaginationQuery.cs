namespace CaffStore.Backend.Interface.Bll.Pagination.Queries
{
	public interface IPaginationQuery
	{
		public int Page { get; set; }

		public int PageSize { get; set; }
	}
}
