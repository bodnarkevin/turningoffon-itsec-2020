using System.Threading.Tasks;
using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;

namespace CaffStore.Backend.Interface.Bll.Services
{
	public interface IAdminCaffItemService
	{
		Task<CaffItemDetailsDto> UpdateCaffItemAsync(long caffItemId, UpdateCaffItemDto updateCaffItem);
		Task DeleteCaffItemAsync(long caffItemId);
	}
}
