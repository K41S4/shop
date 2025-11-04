using AutoMapper;
using CartApp.Models;
using CartApp.WebApi.Dtos;

namespace CartApp.WebApi.MappingProfiles
{
    /// <summary>
    /// Mapping profile for Cart.
    /// </summary>
    public class CartMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartMappingProfile"/> class.
        /// </summary>
        public CartMappingProfile()
        {
            this.CreateMap<Cart, ResponseCart>();
        }
    }
}
