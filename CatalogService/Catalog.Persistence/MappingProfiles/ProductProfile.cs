using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.Persistence.Entities;

namespace Catalog.Persistence.MappingProfiles
{
    /// <summary>
    /// AutoMapper profile for Product entity mappings.
    /// </summary>
    public class ProductProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductProfile"/> class.
        /// </summary>
        public ProductProfile()
        {
            this.CreateMap<Product, ProductEntity>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image.Value))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            this.CreateMap<ProductEntity, Product>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => new Url { Value = src.Image }))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => new Amount { Value = src.Amount }))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Price { Value = src.Price }))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new Name { Value = src.Name }));
        }
    }
}
