using APIGateway.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers;

/// <summary>
/// Aggregate controller for combining product details and properties.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AggregateController : ControllerBase
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IConfiguration configuration;
    private readonly ILogger<AggregateController> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateController"/> class.
    /// </summary>
    /// <param name="httpClientFactory">HTTP client factory.</param>
    /// <param name="configuration">Configuration.</param>
    /// <param name="logger">Logger.</param>
    public AggregateController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<AggregateController> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.configuration = configuration;
        this.logger = logger;
    }

    /// <summary>
    /// Gets aggregated product information including product details and properties.
    /// </summary>
    /// <param name="productId">Product id to get aggregated information for.</param>
    /// <returns>The aggregated product response.</returns>
    [HttpGet("products/{productId}")]
    [Authorize]
    public async Task<IActionResult> GetAggregatedProduct([FromRoute] int productId)
    {
        try
        {
            var catalogServiceUrl = this.configuration["Services:CatalogService"];
            var httpClient = this.httpClientFactory.CreateClient();

            var authHeader = this.HttpContext.Request.Headers.Authorization.ToString();
            if (!string.IsNullOrEmpty(authHeader))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    System.Net.Http.Headers.AuthenticationHeaderValue.Parse(authHeader);
            }

            var productResponse = await httpClient.GetAsync($"{catalogServiceUrl}/api/products/{productId}");

            if (!productResponse.IsSuccessStatusCode)
            {
                return this.StatusCode((int)productResponse.StatusCode, "Error fetching product details.");
            }

            var product = await productResponse.Content.ReadFromJsonAsync<ProductDto>();

            var propertiesResponse = await httpClient.GetAsync($"{catalogServiceUrl}/api/products/{productId}/properties");

            if (!propertiesResponse.IsSuccessStatusCode)
            {
                return this.StatusCode((int)propertiesResponse.StatusCode, "Error fetching product properties.");
            }

            var properties = await propertiesResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            var aggregatedResponse = new
            {
                product,
                properties,
            };

            return this.Ok(aggregatedResponse);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Error aggregating product {ProductId}", productId);
            return this.StatusCode(500, "Internal server error while aggregating product information.");
        }
    }
}