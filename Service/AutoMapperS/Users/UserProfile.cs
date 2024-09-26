using AutoMapper;
using BlogApp.Entity.DTOs.Articles;
using BlogApp.Entity.DTOs.Users;
using BlogApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Service.AutoMapperS.Users
{
     public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser ,UserDto>().ReverseMap();
            CreateMap<AppUser, UserAddDto>().ReverseMap();
            CreateMap<AppUser, UserUpdateDto>().ReverseMap();
            CreateMap<AppUser, UserProfileDto>().ReverseMap();
        }
    }
}




