using CaffStore.Backend.Api.Identity;
using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using CaffStore.Backend.Interface.Bll.Dtos.Error;
using CaffStore.Backend.Interface.Bll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace CaffStore.Backend.Api.Controllers
{
	[Authorize(CaffStorePolicies.AdminOnly)]
	[Route("api/admin/caffItems")]
	[ApiController]
	public class AdminCaffItemController : ControllerBase
	{
		private readonly IAdminCaffItemService _adminCaffItemService;

		public AdminCaffItemController(IAdminCaffItemService adminCaffItemService)
		{
			_adminCaffItemService = adminCaffItemService;
		}

		[HttpPut("{caffItemId}",
			Name = nameof(UpdateCaffItem))]
		[Consumes(MediaTypeNames.Application.Json)]
		[Produces(MediaTypeNames.Application.Json)]
		[ProducesResponseType(typeof(CaffItemDetailsDto), (int)HttpStatusCode.OK)]
		[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
		public Task<CaffItemDetailsDto> UpdateCaffItem([FromRoute] long caffItemId,
			[FromBody] UpdateCaffItemDto updateCaffItem)
		{
			return _adminCaffItemService.UpdateCaffItemAsync(caffItemId, updateCaffItem);
		}

		[HttpDelete("{caffItemId}",
			Name = nameof(DeleteCaffItem))]
		[ProducesResponseType((int)HttpStatusCode.NoContent)]
		public async Task<IActionResult> DeleteCaffItem([FromRoute] long caffItemId)
		{
			await _adminCaffItemService.DeleteCaffItemAsync(caffItemId);

			return NoContent();
		}
	}
}
