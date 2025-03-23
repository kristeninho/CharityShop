using AutoMapper;
using CharityShop.Contracts.Dtos;
using CharityShop.Data.Models;

namespace CharityShop.Services.MappingProfiles;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductResponseDto>()
            .ForMember(dest => dest.Type, opt => 
                opt.MapFrom(src => src.Type.ToString()))
            ;
    }
}