using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.Persistence.Entities;

namespace Catalog.Persistence.MappingProfiles
{
    /// <summary>
    /// AutoMapper profile for Category entity mappings.
    /// </summary>
    public class CategoryProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryProfile"/> class.
        /// </summary>
        public CategoryProfile()
        {
            this.CreateMap<Category, CategoryEntity>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            this.CreateMap<CategoryEntity, Category>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => new Url { Value = src.Image }))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new Name { Value = src.Name }));
        }
    }
}
