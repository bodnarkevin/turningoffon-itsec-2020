using AutoMapper;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.User;

namespace CaffStore.Backend.Bll.AutoMapper.Profiles
{
	public class UserProfile : Profile
	{
		public UserProfile()
		{
			// Entity -> Dto
			CreateMap<User, UserDto>();
			CreateMap<User, UserProfileDto>();

			// Dto -> Entity
			CreateMap<UpdateUserProfileDto, User>();
		}
	}
}
