using Catalog.Core.Entities;
using Catalog.Core.Entities.ValueObjects;
using Catalog.Core.Exceptions;
using Catalog.Core.Repositories;
using Catalog.Core.Services;
using Moq;

namespace Catalog.Core.UnitTests
{
    /// <summary>
    /// Unit tests for the CategoryService class.
    /// </summary>
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> mockCategoryRepository;
        private readonly CategoryService categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryServiceTests"/> class.
        /// </summary>
        public CategoryServiceTests()
        {
            this.mockCategoryRepository = new Mock<ICategoryRepository>();
            this.categoryService = new CategoryService(this.mockCategoryRepository.Object);
        }

        /// <summary>
        /// Tests that AddCategory throws exception when parent category is invalid.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task AddCategory_InvalidParentCategory_ShouldThrow()
        {
            // Arrange
            var product = new Category
            {
                Id = 1,
                ParentCategoryId = 123,
                Name = new Name { Value = "Test category" },
            };

            this.mockCategoryRepository.Setup(x => x.GetCategory(It.IsAny<int>())).ReturnsAsync((Category?)null);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidCategoryException>(() => this.categoryService.AddCategory(product));
            this.mockCategoryRepository.Verify(x => x.AddCategory(It.IsAny<Category>()), Times.Never);
        }

        /// <summary>
        /// Tests that UpdateCategory throws exception when parent category is invalid.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task UpdateCategory_InvalidParentCategory_ShouldThrow()
        {
            // Arrange
            var product = new Category
            {
                Id = 1,
                ParentCategoryId = 123,
                Name = new Name { Value = "Test category" },
            };

            this.mockCategoryRepository.Setup(x => x.GetCategory(It.IsAny<int>())).ReturnsAsync((Category?)null);

            // Act
            // Assert
            await Assert.ThrowsAsync<InvalidCategoryException>(() => this.categoryService.UpdateCategory(product));
            this.mockCategoryRepository.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Never);
        }
    }
}
