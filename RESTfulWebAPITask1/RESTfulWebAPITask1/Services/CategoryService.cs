using RESTfulWebAPITask1.Model;

namespace RESTfulWebAPITask1.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CatalogDbContext _dbContext;
        public CategoryService(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Category> GetAllCategories()
        {
            return _dbContext.Categories.ToList();
        }
        public Category GetCategoryById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return _dbContext.Categories.Find(id);
#pragma warning restore CS8603 // Possible null reference return.
        }
        public void AddCategory(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
        }
        public void UpdateCategory(Category category)
        {
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
        }
        public void DeleteCategory(Category category)
        {
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }
    }
}
