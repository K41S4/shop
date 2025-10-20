using AutoMapper;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Persistence.DBContext;
using Catalog.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CatalogDBContext dbContext;
        private readonly IMapper mapper;

        public CategoryRepository(CatalogDBContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<Category?> GetCategory(int id)
        {
            var categoryEntity = await this.dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            return this.mapper.Map<Category>(categoryEntity);
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var categoryEntities = await this.dbContext.Categories.ToListAsync();
            return this.mapper.Map<IEnumerable<Category>>(categoryEntities);
        }

        public async Task AddCategory(Category category)
        {
            var categoryEntity = this.mapper.Map<CategoryEntity>(category);
            await this.dbContext.Categories.AddAsync(categoryEntity);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task RemoveCategory(int categoryId)
        {
            var categoryEntity = await this.dbContext.Categories.FindAsync(categoryId);
            if (categoryEntity is null) return;

            dbContext.Categories.Remove(categoryEntity);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateCategory(Category category)
        {
            var categoryEntity = await this.dbContext.Categories.FindAsync(category.Id);
            if (categoryEntity is null) return;

            categoryEntity.Name = category.Name.Value;
            categoryEntity.Image = category.Image;
            categoryEntity.ParentCategoryId = category.ParentCategoryId;

            await this.dbContext.SaveChangesAsync();
        }
    }
}
