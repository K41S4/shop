using System.ComponentModel.DataAnnotations;

namespace Catalog.WebAPI.DTOs.Category
{
    /// <summary>
    /// Query parameters for GetProducts endpoint.
    /// </summary>
    public class GetProductsQuery
    {
        /// <summary>
        /// Gets or sets category id to filter products by.
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the requested page for pagination.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Page must be positive number.")]
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the requested limit for pagination.
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Limit must be positive number.")]
        public int Limit { get; set; }
    }
}
