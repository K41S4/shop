using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.Persistence.Entities;

namespace Catalog.Persistence.MappingProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryEntity>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value));

            CreateMap<CategoryEntity, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new Name(src.Name)));
        }
    }
}
