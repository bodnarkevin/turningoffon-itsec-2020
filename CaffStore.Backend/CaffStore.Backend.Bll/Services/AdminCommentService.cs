using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.EntityFrameworkCore;

namespace CaffStore.Backend.Bll.Services
{
	public class AdminCommentService : IAdminCommentService
	{
		private readonly CaffStoreDbContext _context;
		private readonly IMapper _mapper;

		public AdminCommentService(CaffStoreDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
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
