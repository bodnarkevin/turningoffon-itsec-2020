using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface ICommentService
	{
		Task<CommentDto> GetCommentAsync(long commentId);
		Task<CommentDto> UpdateCommentAsync(long commentId, UpdateCommentDto updateComment);
		Task DeleteCommentAsync(long commentId);
	}
}
