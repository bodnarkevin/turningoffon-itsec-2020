using AutoMapper;
using CaffStore.Backend.Dal.Entities;
using CaffStore.Backend.Interface.Bll.Dtos.Comment;

namespace CaffStore.Backend.Bll.AutoMapper.Profiles
{
	public class CommentProfile : Profile
	{
		public CommentProfile()
		{
			// Entity -> Dto
			CreateMap<Comment, CommentDto>();

			// Dto -> Entity
			CreateMap<AddCommentDto, Comment>();
			CreateMap<UpdateCommentDto, Comment>();
		}
	}
}
