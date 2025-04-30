using RESTfulWebAPITask1.Model;

namespace RESTfulWebAPITask1.Services
{
    public class ProductService : IProductService
    {
        private readonly CatalogDbContext _dbContext;
        public ProductService(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<Product> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }
        public Product GetProductById(int id)
        {
            return _dbContext.Products.Find(id);
        }
        public void AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }
        public void UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }
        public void DeleteProduct(Product product)
        {
            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();
        }
        public void DeleteProducts(List<Product> products)
        {
            _dbContext.Products.RemoveRange(products);
            _dbContext.SaveChanges();
        }
    }
}
