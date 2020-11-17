using CaffStore.Backend.Interface.Bll.Dtos.Caff;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using System.Threading.Tasks;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface ICaffItemService
	{
		public Task<PagedResult<CaffItemDto>> ListPagedCaffItemsAsync(IPaginationQuery paginationQuery);
	}
}
