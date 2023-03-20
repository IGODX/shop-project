using MyShopPet.Data;
using MyShopPet.Models.DTOs.UserDTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace MyShopPet.AutoMapperProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<IdentityUser, UserDTO>().ReverseMap();
        }
    }
}
