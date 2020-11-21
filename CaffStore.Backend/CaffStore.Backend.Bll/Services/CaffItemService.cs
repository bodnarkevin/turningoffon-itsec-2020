using AutoMapper;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Bll.Pagination.Extensions;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;
using CaffStore.Backend.Interface.Bll.Dtos.File;
using CaffStore.Backend.Interface.Bll.Enums;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using CaffStore.Backend.Interface.Bll.RequestContext;
using CaffStore.Backend.Interface.Bll.Services;
using CaffStore.Backend.Parser;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CaffStore.Backend.Bll.Services
{
	public class CaffItemService : ICaffItemService
	{
		private readonly CaffStoreDbContext _context;
		private readonly IHttpRequestContext _requestContext;
		private readonly IFileService _fileService;
		private readonly IMapper _mapper;

		public CaffItemService(CaffStoreDbContext context, IHttpRequestContext requestContext, IFileService fileService, IMapper mapper)
		{
			_context = context;
			_requestContext = requestContext;
			_fileService = fileService;
			_mapper = mapper;
		}

		public async Task<PagedResponse<CaffItemDto>> GetPagedCaffItemsAsync(IPagedQuery pagedQuery)
		{
			var response = await _context
				.CaffItems
				.ToPagedAsync<CaffItem, CaffItemDto>(pagedQuery, _mapper);

			await _fileService.SetPreviewFileUris(response.Results.Select(r => r.PreviewFile));

			return response;
		}

		public async Task<CaffItemDetailsDto> GetCaffItemAsync(long caffItemId)
		{
			var caffItemEntity = await _context
				.CaffItems
				.Include(ci => ci.CaffData)
					.ThenInclude(cd => cd.Animations)
					.ThenInclude(a => a.CiffData)
					.ThenInclude(cd => cd.Tags)
					.ThenInclude(t => t.Tag)
				.Include(ci => ci.PreviewFile)
				.Include(ci => ci.CreatedBy)
				.Where(ci => ci.Id == caffItemId)
				.SingleOrDefaultAsync();

			ThrowNotFoundIfNull(caffItemEntity);

			var response = _mapper.Map<CaffItemDetailsDto>(caffItemEntity);

			await _fileService.SetPreviewFileUri(response.PreviewFile);

			return response;
		}

		public async Task<CaffItemDetailsDto> AddCaffItemAsync(AddCaffItemDto caffItem)
		{
			var parseResult = await CaffParser.ParseCaffFileAsync(caffItem.CaffFile.OpenReadStream());

			if (!parseResult.Succeeded)
				throw new CaffStoreBusinessException(parseResult.Message);

			var caffData = _mapper.Map<CaffData>(parseResult.Result);

			// Get all tags
			var parsedTags = caffData.Animations.Select(a => a.CiffData.Tags).SelectMany(t => t).ToList();
			var tagStrings = parsedTags.Select(t => t.Tag.Text).ToHashSet();

			// Prefetch existing tags
			var existingTags = await _context
				.Tags
				.Where(t => tagStrings.Contains(t.Text))
				.ToListAsync();

			// If a tag is already in the database, do not store again, only set reference
			foreach (var parsedTag in parsedTags)
			{
				var existingTag = existingTags
					.FirstOrDefault(t => t.Text == parsedTag.Tag.Text);

				if (existingTag == null)
					existingTags.Add(parsedTag.Tag);
				else
					parsedTag.Tag = existingTag;
			}

			Guid caffFileId = await _fileService.UploadFileAsync(caffItem.CaffFile.OpenReadStream(), ".caff", FileType.Caff);

			Guid previewFileId;
			await using (var previewStream = new MemoryStream())
			{
				parseResult.Preview.Save(previewStream, ImageFormat.Png);
				previewStream.Position = 0;

				previewFileId = await _fileService.UploadFileAsync(previewStream, ".png", FileType.Preview);
			}

			var caffItemEntity = new CaffItem
			{
				Title = caffItem.Title,
				Description = caffItem.Description,
				CaffFileId = caffFileId,
				PreviewFileId = previewFileId,
				CaffData = caffData
			};

			_context.CaffItems.Add(caffItemEntity);

			await _context.SaveChangesAsync();

			return await GetCaffItemAsync(caffItemEntity.Id);
		}

		public async Task<CaffItemDetailsDto> UpdateCaffItemAsync(long caffItemId, UpdateCaffItemDto updateCaffItem)
		{
			var caffItemEntity = await _context
				.CaffItems
				.Where(ci => ci.Id == caffItemId)
				.SingleOrDefaultAsync();

			ThrowNotFoundIfNull(caffItemEntity);
			ThrowForbiddenIfNotCreatedByCurrentUser(caffItemEntity);

			_mapper.Map(updateCaffItem, caffItemEntity);

			await _context.SaveChangesAsync();

			return await GetCaffItemAsync(caffItemId);
		}

		public async Task DeleteCaffItemAsync(long caffItemId)
		{
			var caffItemEntity = await _context
				.CaffItems
				.Where(ci => ci.Id == caffItemId)
				.SingleOrDefaultAsync();

			// Deletion is idempotent
			if (caffItemEntity == null)
				return;

			ThrowForbiddenIfNotCreatedByCurrentUser(caffItemEntity);

			_context.CaffItems.Remove(caffItemEntity);

			await _context.SaveChangesAsync();
		}

		public async Task<FileDto> DownloadCaffFileAsync(long caffItemId)
		{
			var caffItemEntity = await _context
				.CaffItems
				.Include(ci => ci.CaffFile)
				.Where(ci => ci.Id == caffItemId)
				.SingleOrDefaultAsync();

			ThrowNotFoundIfNull(caffItemEntity);

			var response = await _fileService.GetFileAsync(caffItemEntity.CaffFile.Id);

			caffItemEntity.DownloadedTimes++;

			await _context.SaveChangesAsync();

			return response;
		}

		public async Task<CommentDto> AddCaffItemCommentAsync(long caffItemId, AddCommentDto addComment)
		{
			var caffItemEntity = await _context
				.CaffItems
				.Include(ci => ci.Comments)
				.Where(ci => ci.Id == caffItemId)
				.SingleOrDefaultAsync();

			ThrowNotFoundIfNull(caffItemEntity);

			var commentEntity = _mapper.Map<Comment>(addComment);

			caffItemEntity.Comments.Add(new CaffItemComment
			{
				Comment = commentEntity
			});

			await _context.SaveChangesAsync();

			await _context.Entry(commentEntity).Reference(c => c.CreatedBy).LoadAsync();

			return _mapper.Map<CommentDto>(commentEntity);
		}

		public async Task<IEnumerable<CommentDto>> GetCaffItemCommentsAsync(long caffItemId)
		{
			var caffItemEntity = await _context
				.CaffItems
				.Include(ci => ci.Comments)
					.ThenInclude(c => c.Comment)
					.ThenInclude(c => c.CreatedBy)
				.Where(ci => ci.Id == caffItemId)
				.SingleOrDefaultAsync();

			ThrowNotFoundIfNull(caffItemEntity);

			var comments = caffItemEntity.Comments.Select(c => c.Comment);

			return _mapper.Map<IEnumerable<CommentDto>>(comments);
		}

		private void ThrowNotFoundIfNull(CaffItem caffItem)
		{
			if (caffItem == null)
				throw new CaffStoreNotFoundException();
		}

		private void ThrowForbiddenIfNotCreatedByCurrentUser(CaffItem caffItem)
		{
			if (caffItem.CreatedById != _requestContext.CurrentUserId.Value)
				throw new CaffStoreForbiddenException("CaffItem was not created by current user");
		}
	}
}
