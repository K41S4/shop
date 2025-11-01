using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.WebAPI.DTOs.Category;

namespace Catalog.WebAPI.Mapping
{
    /// <summary>
    /// AutoMapper profile for Category DTOs mappings.
    /// </summary>
    public class CategoryDtosProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryDtosProfile"/> class.
        /// </summary>
        public CategoryDtosProfile()
        {
            this.CreateMap<Category, ResponseCategoryDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image == null ? null : src.Image.Value))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            this.CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => new Url { Value = src.Image }))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new Name { Value = src.Name }));

            this.CreateMap<UpdateCategoryDto, Category>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => new Url { Value = src.Image }))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new Name { Value = src.Name }));
        }
    }
}
