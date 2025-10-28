using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Persistence.DBContext;
using Catalog.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Repositories
{
    /// <summary>
    /// Repository implementation for category data access.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogDBContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public CategoryRepository(CatalogDBContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<Category?> GetCategory(int id)
        {
            var categoryEntity = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            return this.mapper.Map<Category>(categoryEntity);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categoryEntities = await this.dbContext.Categories.ToListAsync();
            return this.mapper.Map<IEnumerable<Category>>(categoryEntities);
        }

        /// <inheritdoc/>
        public async Task AddCategory(Category category)
        {
            var categoryEntity = this.mapper.Map<CategoryEntity>(category);
            await this.dbContext.Categories.AddAsync(categoryEntity);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task RemoveCategory(int categoryId)
        {
            var categoryEntity = await this.dbContext.Categories.FindAsync(categoryId);
            if (categoryEntity is null)
            {
                return;
            }

            this.dbContext.Categories.Remove(categoryEntity);
            await this.dbContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateCategory(Category category)
        {
            var categoryEntity = await this.dbContext.Categories.FindAsync(category.Id);
            if (categoryEntity is null)
            {
                return;
            }

            categoryEntity.Name = category.Name.Value!;
            categoryEntity.Image = category.Image?.Value;
            categoryEntity.ParentCategoryId = category.ParentCategoryId;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
