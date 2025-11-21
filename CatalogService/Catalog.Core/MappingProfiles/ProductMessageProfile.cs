using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Messaging;

namespace Catalog.Core.MappingProfiles
{
    /// <summary>
    /// AutoMapper profile for Product Message entity mappings.
    /// </summary>
    public class ProductMessageProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductMessageProfile"/> class.
        /// </summary>
        public ProductMessageProfile()
        {
            this.CreateMap<Product, ProductUpdatedMessage>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price.Value))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image == null ? null : src.Image.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));
        }
    }
}
