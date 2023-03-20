using MyShopPet.Data;
using MyShopPet.Models.DTOs.UserDTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace MyShopPet.AutoMapperProfile
{
    public class EditUserProfile : Profile
    {
        public EditUserProfile()
        {
            CreateMap<IdentityUser, EditUserDTO>().ReverseMap();
        }
    }
}
