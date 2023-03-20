using MyShopPet.Data;
using MyShopPet.Models.DTOs.UserDTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace MyShopPet.AutoMapperProfile
{
    public class CreateUserProfile : Profile
    {
        public CreateUserProfile()
        {
            CreateMap<IdentityUser, CreateUserDTO>().ReverseMap();
        }
    }
}
