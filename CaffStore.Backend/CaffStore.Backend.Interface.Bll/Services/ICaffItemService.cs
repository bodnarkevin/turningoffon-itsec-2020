using CaffStore.Backend.Interface.Bll.Dtos.Caff;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Dtos.File;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface ICaffItemService
	{
		public Task<PagedResponse<CaffItemDto>> GetPagedCaffItemsAsync(IPagedQuery pagedQuery);
		public Task<CaffItemDetailsDto> GetCaffItemAsync(long caffItemId);
		Task<CaffItemDetailsDto> AddCaffItemAsync(AddCaffItemDto caffItem);
		Task<FileDto> DownloadCaffFileAsync(long caffItemId);
	}
}
