using AutoMapper;
using MyShopPet.Data;
using MyShopPet.Models.ViewModels.ProductViewModel;

namespace MyShopPet.AutoMapperProfile
{
    public class CreateProductProfile : Profile
    {
        public CreateProductProfile()
        {
            CreateMap<Product, CreateProductViewModel>().ReverseMap().
            ForMember(dest => dest.Photos, act => act.Ignore());
        }
    }
}
