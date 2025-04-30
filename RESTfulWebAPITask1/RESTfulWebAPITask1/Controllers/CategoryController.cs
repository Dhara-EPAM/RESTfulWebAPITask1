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
        public IActionResult Get()
        {
            var category = _dbContext.Categories.ToList();
            if (category == null)
                return NotFound(new { Message = "There is no any category available" });

            return Ok(category);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _dbContext.Categories.Find(id);
            if (category == null)
                return NotFound(new { Message = "Category not found" });

            return Ok(category);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public IActionResult Post(Category category)
        {
            if (category == null)
                return BadRequest("Category details are required");

            //Parent category id logic
            if(category.ParentCategoryId != null && category.ParentCategoryId > 0)
            {
                var parentCategory = _dbContext.Categories.Find(category.ParentCategoryId);
                if (parentCategory == null)
                    return NotFound(new { Message = "Parent Category Id not found" });
            }
            //
            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return Ok(new { Message = "Category has been added successfully", Category = category });
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Category category)
        {
            if (category == null)
                return BadRequest("Category details are required");

            //Parent category id logic
            if (category.ParentCategoryId != null && category.ParentCategoryId > 0)
            {
                var parentCategory = _dbContext.Categories.Find(category.ParentCategoryId);
                if (parentCategory == null)
                    return NotFound(new { Message = "Parent Category Id not found" });
            }
            //
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();

            return Ok(new { Message = "Category has been added successfully", Category = category });
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _dbContext.Categories.Find(id);
            if (entity == null)
                return NotFound(new { Message = "Category not found" });
            
            //Delete related products
            var products = _dbContext.Products.ToList().Where(x => x.CategoryId == id);
            _dbContext.Products.RemoveRange(products);
            _dbContext.SaveChanges();

            //Delete category
            _dbContext.Categories.Remove(entity);
            _dbContext.SaveChanges();

             return Ok(new { Message = "Category deleted successfully" });
         
        }
    }
}
