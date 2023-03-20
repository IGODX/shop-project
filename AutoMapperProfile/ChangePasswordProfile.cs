using MyShopPet.Data;
using MyShopPet.Models.DTOs.UserDTOs;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace MyShopPet.AutoMapperProfile
{
    public class ChangePasswordProfile : Profile
    {
        public ChangePasswordProfile()
        {
            CreateMap<IdentityUser, ChangePasswordDTO>().ReverseMap();
        }
    }
}
