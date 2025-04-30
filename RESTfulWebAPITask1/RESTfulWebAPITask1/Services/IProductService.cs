using RESTfulWebAPITask1.Model;

namespace RESTfulWebAPITask1.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
        void DeleteProducts(List<Product> products);
    }
}
