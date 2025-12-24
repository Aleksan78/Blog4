using AutoMapper;
using Blog2.BLL.ViewModels.Comments;
using Blog2.BLL.ViewModels.Posts;
using Blog2.BLL.ViewModels.Roles;
using Blog2.BLL.ViewModels.Tags;
using Blog2.BLL.ViewModels.User;
using Blog2.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;

namespace Blog2.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Маппинг пользователя при регистрации
            CreateMap<UserRegisterViewModel, User>();
            //.ForMember(x => x.Email, opt => opt.MapFrom(c => c.Email))
            //.ForMember(x => x.UserName, opt => opt.MapFrom(c => c.UserName));

            // Маппинг при редактировании пользователя (двусторонний)
            CreateMap<UserEditViewModel, User>().ReverseMap();

            // Комментарии
            CreateMap<CommentCreateViewModel, Comment>();
            CreateMap<CommentEditViewModel, Comment>();

            // Посты (добавляем ReverseMap, чтобы данные подтягивались в форму редактирования)
            CreateMap<PostCreateViewModel, Post>();
            //CreateMap<PostEditViewModel, Post>();
            CreateMap<PostEditViewModel, Post>().ReverseMap();

            //Теги
            CreateMap<TagCreateViewModel, Tag>();
            //CreateMap<TagEditViewModel, Tag>();
            CreateMap<TagEditViewModel, Tag>().ReverseMap();
            CreateMap<TagViewModel, Tag>();

            //Роли
            CreateMap<RoleCreateViewModel, IdentityRole>();
             CreateMap<RoleEditViewModel, IdentityRole>().ReverseMap();


            //CreateMap<UserEditViewModel, User>();
        }
    }
}
