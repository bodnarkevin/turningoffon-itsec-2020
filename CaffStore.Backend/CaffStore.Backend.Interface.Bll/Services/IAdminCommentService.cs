using System.Threading.Tasks;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface IAdminCommentService
	{
		Task DeleteCommentAsync(long commentId);
	}
}
