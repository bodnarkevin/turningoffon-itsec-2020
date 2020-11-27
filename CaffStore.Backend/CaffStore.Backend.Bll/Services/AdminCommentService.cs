using CaffStore.Backend.Dal;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CaffStore.Backend.Bll.Services
{
	public class AdminCommentService : IAdminCommentService
	{
		private readonly CaffStoreDbContext _context;

		public AdminCommentService(CaffStoreDbContext context)
		{
			_context = context;
		}

		public async Task DeleteCommentAsync(long commentId)
		{
			var commentEntity = await _context
				.Comments
				.Where(c => c.Id == commentId)
				.SingleOrDefaultAsync();

			// Deletion is idempotent
			if (commentEntity == null)
				return;

			_context.Comments.Remove(commentEntity);

			await _context.SaveChangesAsync();
		}
	}
}
