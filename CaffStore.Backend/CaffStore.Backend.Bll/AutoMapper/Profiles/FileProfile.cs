using AutoMapper;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.File;

namespace CaffStore.Backend.Bll.AutoMapper.Profiles
{
	public class FileProfile : Profile
	{
		public FileProfile()
		{
			// Entity -> Dto
			CreateMap<File, FileDto>()
				.IncludeAllDerived();
		}
	}
}
