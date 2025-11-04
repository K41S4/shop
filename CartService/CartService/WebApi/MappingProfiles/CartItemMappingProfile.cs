using AutoMapper;
using CartApp.Models;
using CartApp.WebApi.Dtos;

namespace CartApp.WebApi.MappingProfiles
{
    /// <summary>
    /// Mapping profile for CartItem.
    /// </summary>
    public class CartItemMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CartItemMappingProfile"/> class.
        /// </summary>
        public CartItemMappingProfile()
        {
            this.CreateMap<CartItem, ResponseCartItem>();
            this.CreateMap<AddCartItem, CartItem>();
        }
    }
}
