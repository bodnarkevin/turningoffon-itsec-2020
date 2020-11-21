using AutoMapper;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.CaffItem;
using System.Linq;

namespace CaffStore.Backend.Bll.AutoMapper.Profiles
{
	public class CaffProfile : Profile
	{
		public CaffProfile()
		{
			// Entity -> Dto
			CreateMap<CaffItem, CaffItemDto>();
			CreateMap<CaffItem, CaffItemDetailsDto>();

			CreateMap<CaffData, CaffDataDto>();
			CreateMap<CaffAnimationData, CaffAnimationDataDto>();
			CreateMap<CiffData, CiffDataDto>()
				.ForMember(dto => dto.Tags, conf =>
					conf.MapFrom(cd => cd.Tags.Select(t => t.Tag.Text)));

			// Dto -> Entity
			CreateMap<CaffDataDto, CaffData>();
			CreateMap<CaffAnimationDataDto, CaffAnimationData>();
			CreateMap<CiffDataDto, CiffData>()
				.ForMember(cd => cd.Tags, conf =>
					conf.MapFrom(dto => dto.Tags.Select(t =>
						new CiffDataTag
						{
							Tag = new Tag
							{
								Text = t
							}
						})));
			CreateMap<UpdateCaffItemDto, CaffItem>();
		}
	}
}
