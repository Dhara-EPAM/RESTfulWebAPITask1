using Microsoft.AspNetCore.Mvc;
using RESTfulWebAPITask1.Model;
using RESTfulWebAPITask1.Services;

namespace RESTfulWebAPITask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public IActionResult Get()
        {
            var category = _categoryService.GetAllCategories();
            if (category == null)
                return NotFound(new { Message = "There is no any category available" });

            return Ok(category);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = _categoryService.GetCategoryById(id);
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
                var parentCategory = _categoryService.GetCategoryById((int)category.ParentCategoryId);
                if (parentCategory == null)
                    return NotFound(new { Message = "Parent Category Id not found" });
            }
            //Add
            _categoryService.AddCategory(category);

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
                var parentCategory = _categoryService.GetCategoryById((int)category.ParentCategoryId);
                if (parentCategory == null)
                    return NotFound(new { Message = "Parent Category Id not found" });
            }
            //update
            _categoryService.UpdateCategory(category);

            return Ok(new { Message = "Category has been added successfully", Category = category });
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var entity = _categoryService.GetCategoryById(id);
            if (entity == null)
                return NotFound(new { Message = "Category not found" });
            
            //Delete related products
            var products = _productService.GetAllProducts()?.Where(x => x.CategoryId == id)?.ToList();
            _productService.DeleteProducts(products);

            //Delete category
            _categoryService.DeleteCategory(entity);

             return Ok(new { Message = "Category deleted successfully" });
         
        }
    }
}
