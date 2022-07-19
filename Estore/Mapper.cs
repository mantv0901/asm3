using AutoMapper;
using BusinessLayer.Models;
using Estore.DTOs;
namespace Estore
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(des => des.CategoryName, act => act.MapFrom(src => src.Category.CategoryName));
        }
    }
}
