using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RESTfulWebAPITask1.Model;
using RESTfulWebAPITask1.Services;
using Shared;

namespace RESTfulWebAPITask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly RabbitMqPublisher _publisher;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        public ProductController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _publisher = new RabbitMqPublisher();
        }


        // GET: api/<ProductController>
        [HttpGet]
        // Read - Accessible by all roles
        public IActionResult Get()
        {
            var product = _productService.GetAllProducts();
            if (product == null)
                return NotFound(new { Message = "There is no any product available" });

            return Ok(product);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        // Read - Accessible by all roles 
        public IActionResult Get(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
                return NotFound(new { Message = "Product not found" });

            return Ok(product);
        }


        // POST api/<ProductController>
        [HttpPost]
        [Authorize(Roles = "Manager")] // Restricted to Manager role only
        public IActionResult Post(Product product)
        {
            if (product == null)
                return BadRequest("Product details are required");

            //Category logic
            if (product.CategoryId != null && product.CategoryId > 0)
            {
                var category = _categoryService.GetCategoryById(product.CategoryId);
                if (category == null)
                    return NotFound(new { Message = "Category Id not found" });
            }
            else
            {
                return BadRequest("Category Id is required");
            }
            //
            _productService.AddProduct(product);

            return Ok(new { Message = "Product has been added successfully", Product = product });
        }


        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")] // Restricted to Manager role only
        public IActionResult Put(int id, Product product)
        {
            if (product == null)
                return BadRequest("Product details are required");
            
            //Category logic
            if (product.CategoryId != null && product.CategoryId > 0)
            {
                var category = _categoryService.GetCategoryById(product.CategoryId);
                if (category == null)
                    return NotFound(new { Message = "Category Id not found" });
            }
            else
            {
                return BadRequest("Category Id is required");
            }
            //
            _productService.UpdateProduct(product);

            return Ok(new { Message = "Product has been added successfully", Product = product });
        }

        // PUT api/<ProductController>/5
        [HttpPut("UpdateProductWithCart{id}")]
        public IActionResult UpdateProductWithCart(int id, Product product)
        {
            if (product == null)
                return BadRequest("Product details are required");

            //Category logic
            if (product.CategoryId != null && product.CategoryId > 0)
            {
                var category = _categoryService.GetCategoryById(product.CategoryId);
                if (category == null)
                    return NotFound(new { Message = "Category Id not found" });
            }
            else
            {
                return BadRequest("Category Id is required");
            }
            //Update catalog product
            product.Id = id;
            _productService.UpdateProduct(product);

            //Call RabbitMq Publisher to call cart service to update product there
            ItemUpdatedEvent itemUpdated = new ItemUpdatedEvent();
            itemUpdated.Name = product.Name;
            itemUpdated.Price = product.Price;
            itemUpdated.ItemId = id;
            _publisher.PublishCatalogItemUpdatedEvent(itemUpdated);

            return Ok(new { Message = "Product has been updated successfully and event published!", Product = product });
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")] // Restricted to Manager role only
        public IActionResult Delete(int id)
        {
            var entity = _productService.GetProductById(id);
            if (entity == null)
                return NotFound(new { Message = "Product not found" });

            _productService.DeleteProduct(entity);

            return Ok(new { Message = "Product deleted successfully" });
        }
    }
}
