using AutoMapper;
using CaffStore.Backend.Bll.Exceptions;
using CaffStore.Backend.Dal;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CaffStore.Backend.Bll.Services
{
	public class AdminCaffItemService : IAdminCaffItemService
	{
		private readonly CaffStoreDbContext _context;
		private readonly IFileService _fileService;
		private readonly IMapper _mapper;

		public AdminCaffItemService(CaffStoreDbContext context, IFileService fileService, IMapper mapper)
		{
			_context = context;
			_fileService = fileService;
			_mapper = mapper;
		}

		public async Task<CaffItemDetailsDto> UpdateCaffItemAsync(long caffItemId, UpdateCaffItemDto updateCaffItem)
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

			_mapper.Map(updateCaffItem, caffItemEntity);

			await _context.SaveChangesAsync();

			var response = _mapper.Map<CaffItemDetailsDto>(caffItemEntity);

			await _fileService.SetPreviewFileUri(response.PreviewFile);

			return response;
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

			_context.CaffItems.Remove(caffItemEntity);

			await _context.SaveChangesAsync();
		}

		private void ThrowNotFoundIfNull(CaffItem caffItem)
		{
			if (caffItem == null)
				throw new CaffStoreNotFoundException();
		}
	}
}
