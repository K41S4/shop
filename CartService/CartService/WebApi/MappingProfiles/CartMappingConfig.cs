using CartApp.Models;
using CartApp.WebApi.Dtos;
using Mapster;

namespace CartApp.WebApi.MappingProfiles;

/// <summary>
/// Mapping configuration for Cart.
/// </summary>
public class CartMappingConfig : IRegister
{
    /// <summary>
    /// Registers the mapping configuration.
    /// </summary>
    /// <param name="config">The Mapster type adapter configuration.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Cart, ResponseCart>();
    }
}