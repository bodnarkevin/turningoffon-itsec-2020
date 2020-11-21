using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;
using CaffStore.Backend.Interface.Bll.RequestContext;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.EntityFrameworkCore;

namespace CaffStore.Backend.Bll.Services
{
	public class CommentService : ICommentService
	{
		private readonly CaffStoreDbContext _context;
		private readonly IHttpRequestContext _requestContext;
		private readonly IMapper _mapper;

		public CommentService(CaffStoreDbContext context, IHttpRequestContext requestContext, IMapper mapper)
		{
			_context = context;
			_requestContext = requestContext;
			_mapper = mapper;
		}

		public async Task<CommentDto> GetCommentAsync(long commentId)
		{
			var commentEntity = await _context
				.Comments
				.Include(c => c.CreatedBy)
				.Where(c => c.Id == commentId)
				.SingleOrDefaultAsync();

			ThrowNotFoundIfNull(commentEntity);

			return _mapper.Map<CommentDto>(commentEntity);
		}
		
		public async Task<CommentDto> UpdateMyCommentAsync(long commentId, UpdateCommentDto updateComment)
		{
			var commentEntity = await _context
				.Comments
				.Include(c => c.CreatedBy)
				.Where(c => c.Id == commentId)
				.SingleOrDefaultAsync();

			ThrowNotFoundIfNull(commentEntity);
			ThrowForbiddenIfNotCreatedByCurrentUser(commentEntity);

			_mapper.Map(updateComment, commentEntity);

			await _context.SaveChangesAsync();

			return _mapper.Map<CommentDto>(commentEntity);
		}
		
		public async Task DeleteMyCommentAsync(long commentId)
		{
			var commentEntity = await _context
				.Comments
				.Where(c => c.Id == commentId)
				.SingleOrDefaultAsync();

			// Deletion is idempotent
			if (commentEntity == null)
				return;

			ThrowForbiddenIfNotCreatedByCurrentUser(commentEntity);

			_context.Comments.Remove(commentEntity);

			await _context.SaveChangesAsync();
		}

		private void ThrowNotFoundIfNull(Comment comment)
		{
			if (comment == null)
				throw new CaffStoreNotFoundException();
		}

		private void ThrowForbiddenIfNotCreatedByCurrentUser(Comment comment)
		{
			if (comment.CreatedById != _requestContext.CurrentUserId.Value)
				throw new CaffStoreForbiddenException("CaffItem was not created by current user");
		}
	}
}
