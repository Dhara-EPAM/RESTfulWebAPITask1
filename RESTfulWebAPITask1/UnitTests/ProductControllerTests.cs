using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RESTfulWebAPITask1.Controllers;
using RESTfulWebAPITask1.Model;
using RESTfulWebAPITask1.Services;

namespace UnitTests
{
    public class ProductControllerTests
    {
        private ProductController _controller;
        private Mock<ICategoryService> _categoryService;
        private Mock<IProductService> _productService;

        [SetUp]
        public void Setup()
        {
            _categoryService = new Mock<ICategoryService>();
            _productService = new Mock<IProductService>();
            _controller = new ProductController(_categoryService.Object, _productService.Object);
        }

        [Test]
        public void Get_WhenProductsExist_ReturnsOkResultWithProductList()
        {
            var products = new List<Product>
            {
                new Product { 
                    Id = 1, CategoryId = 1, Name = "Product1", 
                    Description = "", 
                    Image = "",
                    Price = 1000,
                    Amount = 1000
                },
                new Product { 
                    Id = 2, CategoryId = 1, Name = "Product2",
                    Description = "",
                    Image = "",
                    Price = 1000,
                    Amount = 1000
                }
            };
            _productService.Setup(s => s.GetAllProducts()).Returns(products);

            var result = _controller.Get();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result); // Verify result is 200 OK
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(products, okResult.Value); // Verify the returned products list
        }

        [Test]
        public void AddProduct_ReturnsCreatedResult()
        {
            var product = new Product
            {
                Id = 1,
                Name = "Product1",
                CategoryId = 1,
                Description = "",
                Image = "",
                Price = 1000,
                Amount = 1000
            };

            var category = new Category { Id = 1, Name = "Category1" }; // Mocked category
            _categoryService.Setup(service => service.GetCategoryById(1)).Returns(category); // Category exists


            var result = _controller.Post(product);

            // Assert
            var okResult = result as OkObjectResult;

            Assert.IsNotNull(okResult);
            
            var json = JsonConvert.SerializeObject(okResult.Value);
            var jObject = JObject.Parse(json);

            Assert.AreEqual(product.Name, jObject["Product"]["Name"].ToString()); // Verify product details

            // Verify that the product was added
            _productService.Verify(service => service.AddProduct(product), Times.Once);
        }


        [Test]
        public void UpdateProduct_ReturnsUpdatedResult()
        {
            int id = 1;
            var products = new Product
            {
                Id = 1,
                CategoryId = 1,
                Name = "Product1",
                Description="",
                Image="",
                Price = 1000,
                Amount =1000
            };
            var category = new Category { Id = 1, Name = "Category1" }; // Mocked category
            _categoryService.Setup(service => service.GetCategoryById(1)).Returns(category); // Category exists

            _productService.Setup(service => service.UpdateProduct(products));

            var response = _controller.Put(id, products);

            // Assert
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult);

            var json = JsonConvert.SerializeObject(okResult.Value);
            var jObject = JObject.Parse(json);

            Assert.AreEqual(products.Name, jObject["Product"]["Name"].ToString());
        }

        [Test]
        public void DeleteProduct_ReturnsOk_WhenIsDeleted()
        {
            int id = 1;
            var products = new Product
            {
                Id = 1,
                CategoryId = 0,
                Name = "Product1",
                Description = "",
                Image = "",
                Price = 1000,
                Amount = 1000
            };

            _productService.Setup(service => service.GetProductById(id)).Returns(products);
            _productService.Setup(service => service.DeleteProduct(products)).Verifiable();

            var response = _controller.Delete(id);

            // Assert
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult);

            var responseString = okResult.Value.ToString();
            Assert.IsTrue(responseString.Contains("Product deleted successfully"));
        }

    }
}
