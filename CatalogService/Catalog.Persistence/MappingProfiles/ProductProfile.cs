using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.Persistence.Entities;

namespace Catalog.Persistence.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductEntity>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            CreateMap<ProductEntity, Product>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => new Amount(src.Amount)))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Price(src.Price)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new Name(src.Name)));
        }
    }
}
