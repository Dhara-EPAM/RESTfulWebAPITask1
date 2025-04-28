using Microsoft.AspNetCore.Mvc;
using RESTfulWebAPITask1.Model;

namespace RESTfulWebAPITask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CatalogDbContext _dbContext;
        public CategoryController(CatalogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _dbContext.Categories.ToList();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return _dbContext.Categories.Find(id);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public void Post(Category category)
        {
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, Category category)
        {
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var entity = _dbContext.Categories.Find(id);
            if (entity != null)
            {
                //Delete related products
                var products = _dbContext.Products.ToList().Where(x => x.CategoryId == id);
                _dbContext.Products.RemoveRange(products);
                _dbContext.SaveChanges();

                //Delete category
                _dbContext.Categories.Remove(entity);
                _dbContext.SaveChanges();
            }
        }
    }
}
