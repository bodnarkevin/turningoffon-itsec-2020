using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using AutoMapper;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Bll.Pagination.Extensions;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.Caff;
using CaffStore.Backend.Interface.Bll.Pagination.Queries;
using CaffStore.Backend.Interface.Bll.Pagination.Responses;
using CaffStore.Backend.Interface.Bll.Services;
using CaffStore.Backend.Parser;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Enums;

namespace CaffStore.Backend.Bll.Services
{
	public class CaffItemService : ICaffItemService
	{
		private readonly CaffStoreDbContext _context;

		private readonly IFileService _fileService;

		private readonly IMapper _mapper;

		public CaffItemService(CaffStoreDbContext context, IMapper mapper)
		{
			_context = context;
			//_fileService = fileService;
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

			var response =  _mapper.Map<CaffItemDetailsDto>(caffItemEntity);

			//await _fileService.SetPreviewFileUri(response.PreviewFile);

			return response;
		}

		public async Task<CaffItemDetailsDto> AddCaffItemAsync(AddCaffItemDto caffItem)
		{
			var parseResult = await CaffParser.ParseCaffFileAsync(caffItem.CaffFile.OpenReadStream());

			if (!parseResult.Succeeded)
				throw new CaffStoreBusinessException(parseResult.Message);

			//Guid caffFileId = await _fileService.UploadFileAsync(caffItem.CaffFile.OpenReadStream(), ".caff", FileType.Caff);

			//Guid previewFileId;
			//await using (var previewStream = new MemoryStream())
			//{
			//	parseResult.Preview.Save(previewStream, ImageFormat.Png);
			//	previewFileId = await _fileService.UploadFileAsync(previewStream, ".png", FileType.Preview);
			//}

			var caffData = _mapper.Map<CaffData>(parseResult.Result);

			//List<CaffAnimationData> animations = new List<CaffAnimationData>();

			//var caffData = new CaffData
			//{
			//	Creator = parseResult.Result.Creator,
			//	Creation = parseResult.Result.Creation,
			//	Animations = animations,
			//};

			var caffItemEntity = new CaffItem
			{
				Title = caffItem.Title,
				Description = caffItem.Description,
				//CaffFileId = caffFileId,
				//PreviewFileId = previewFileId,
				CaffData = caffData
			};

			_context.CaffItems.Add(caffItemEntity);

			await _context.SaveChangesAsync();

			return await GetCaffItemAsync(caffItemEntity.Id);
		}



		private void ThrowNotFoundIfNull(CaffItem caffItem)
		{
			if (caffItem == null)
				throw new CaffStoreNotFoundException();
		}
	}
}
