using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.WebAPI.DTOs.Product;

namespace Catalog.WebAPI.Mapping
{
    /// <summary>
    /// AutoMapper profile for Product DTOs mappings.
    /// </summary>
    public class ProductDtosProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductDtosProfile"/> class.
        /// </summary>
        public ProductDtosProfile()
        {
            this.CreateMap<Product, ResponseProductDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image.Value))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            this.CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => new Url { Value = src.Image }))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => new Amount { Value = src.Amount!.Value }))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Price { Value = src.Price!.Value }))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new Name { Value = src.Name }));

            this.CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => new Url { Value = src.Image }))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => new Amount { Value = src.Amount!.Value }))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => new Price { Value = src.Price!.Value }))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new Name { Value = src.Name }));
        }
    }
}
