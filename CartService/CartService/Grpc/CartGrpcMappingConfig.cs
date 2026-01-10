using CartApp.Models;
using Mapster;

namespace CartApp.Grpc;

/// <summary>
/// Mapping configuration for gRPC Cart messages.
/// </summary>
public class CartGrpcMappingConfig : IRegister
{
    /// <summary>
    /// Registers the mapping configuration.
    /// </summary>
    /// <param name="config">The Mapster type adapter configuration.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CartItemRequest, CartItem>()
            .Map(dest => dest.Price, src => (decimal)src.Price)
            .Map(dest => dest.ImageUrl, src => string.IsNullOrEmpty(src.ImageUrl) ? null : src.ImageUrl)
            .Map(dest => dest.ImageDescription, src => string.IsNullOrEmpty(src.ImageDescription) ? null : src.ImageDescription);

        config.NewConfig<CartItem, CartItemResponse>()
            .Map(dest => dest.Price, src => (double)src.Price)
            .Map(dest => dest.Name, src => src.Name ?? string.Empty)
            .Map(dest => dest.ImageUrl, src => src.ImageUrl ?? string.Empty)
            .Map(dest => dest.ImageDescription, src => src.ImageDescription ?? string.Empty);
    }
}