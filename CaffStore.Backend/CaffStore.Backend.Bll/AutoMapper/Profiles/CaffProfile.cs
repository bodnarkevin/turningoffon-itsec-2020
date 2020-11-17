using AutoMapper;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.Caff;

namespace CaffStore.Backend.Bll.AutoMapper.Profiles
{
	public class CaffProfile : Profile
	{
		public CaffProfile()
		{
			// Entity -> Dto
			CreateMap<CaffItem, CaffItemDto>();

			// Dto -> Entity
		}
	}
}
