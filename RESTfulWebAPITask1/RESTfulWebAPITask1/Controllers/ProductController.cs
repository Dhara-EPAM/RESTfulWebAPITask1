using Microsoft.AspNetCore.Mvc;
using RESTfulWebAPITask1.Model;

namespace RESTfulWebAPITask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly CatalogDbContext _dbContext;
        public ProductController(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // GET: api/<ProductController>
        [HttpGet]
        public IEnumerable<Product> Get()
        {
            return _dbContext.Products.ToList();
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _dbContext.Products.Find(id);
        }


        // POST api/<ProductController>
        [HttpPost]
        public void Post(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }


        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, Product product)
        {
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();
        }


        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var entity = _dbContext.Products.Find(id);
            if (entity != null)
            {
                _dbContext.Products.Remove(entity);
                _dbContext.SaveChanges();
            }
        }
    }
}
