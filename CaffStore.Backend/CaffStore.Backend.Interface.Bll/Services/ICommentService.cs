using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface ICommentService
	{
		Task<CommentDto> GetCommentAsync(long commentId);
		Task<CommentDto> UpdateMyCommentAsync(long commentId, UpdateCommentDto updateComment);
		Task DeleteMyCommentAsync(long commentId);
	}
}
