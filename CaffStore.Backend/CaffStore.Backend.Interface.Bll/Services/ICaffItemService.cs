using System.Collections.Generic;
using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;
using CaffStore.Backend.Interface.Bll.Dtos.File;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface ICaffItemService
	{
		Task<PagedResponse<CaffItemDto>> GetPagedCaffItemsAsync(IPagedQuery pagedQuery);
		Task<CaffItemDetailsDto> GetCaffItemAsync(long caffItemId);
		Task<CaffItemDetailsDto> AddCaffItemAsync(AddCaffItemDto caffItem);
		Task<CaffItemDetailsDto> UpdateCaffItemAsync(long caffItemId, UpdateCaffItemDto updateCaffItem);
		Task DeleteCaffItemAsync(long caffItemId);
		Task<FileDto> DownloadCaffFileAsync(long caffItemId);
		Task<IEnumerable<CommentDto>> GetCaffItemCommentsAsync(long caffItemId);
		Task<CommentDto> AddCaffItemCommentAsync(long caffItemId, AddCommentDto addComment);
	}
}
