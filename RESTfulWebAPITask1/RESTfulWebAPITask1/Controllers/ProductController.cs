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
        public IActionResult Get()
        {
            var product = _dbContext.Products.ToList();
            if (product == null)
                return NotFound(new { Message = "There is no any product available" });

            return Ok(product);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
                return NotFound(new { Message = "Product not found" });

            return Ok(product);
        }


        // POST api/<ProductController>
        [HttpPost]
        public IActionResult Post(Product product)
        {
            if (product == null)
                return BadRequest("Product details are required");

            //Category logic
            if (product.CategoryId != null && product.CategoryId > 0)
            {
                var category = _dbContext.Categories.Find(product.CategoryId);
                if (category == null)
                    return NotFound(new { Message = "Category Id not found" });
            }
            //
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();

            return Ok(new { Message = "Product has been added successfully", Product = product });
        }


        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Product product)
        {
            if (product == null)
                return BadRequest("Product details are required");
            
            //Category logic
            if (product.CategoryId != null && product.CategoryId > 0)
            {
                var category = _dbContext.Categories.Find(product.CategoryId);
                if (category == null)
                    return NotFound(new { Message = "Category Id not found" });
            }
            //
            _dbContext.Products.Update(product);
            _dbContext.SaveChanges();

            return Ok(new { Message = "Product has been added successfully", Product = product });
        }


        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _dbContext.Products.Find(id);
            if (entity == null)
                return NotFound(new { Message = "Product not found" });
            
            _dbContext.Products.Remove(entity);
            _dbContext.SaveChanges();

            return Ok(new { Message = "Product deleted successfully" });
        }
    }
}
