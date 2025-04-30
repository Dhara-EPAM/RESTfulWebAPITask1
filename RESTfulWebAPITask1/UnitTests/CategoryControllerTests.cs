using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RESTfulWebAPITask1;
using RESTfulWebAPITask1.Controllers;
using RESTfulWebAPITask1.Model;
using RESTfulWebAPITask1.Services;

namespace UnitTests
{
    public class CategoryControllerTests
    {
        private CategoryController _controller;
        private Mock<ICategoryService> _categoryService;
        private Mock<IProductService> _productService;

        [SetUp]
        public void Setup()
        {
            _categoryService = new Mock<ICategoryService>();
            _productService = new Mock<IProductService>();
            _controller = new CategoryController(_categoryService.Object, _productService.Object);
        }

        [Test]
        public void Get_WhenCategoriesExist_ReturnsOkResultWithCategoryList()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, ParentCategoryId = 0, Name = "Category1" },
                new Category { Id = 2, ParentCategoryId = 0, Name = "Category2" }
            };
            _categoryService.Setup(s => s.GetAllCategories()).Returns(categories);

            var result = _controller.Get();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result); // Verify result is 200 OK
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(categories, okResult.Value); // Verify the returned categories list
        }

        [Test]
        public void AddCategory_ReturnsCreatedResult()
        {
            var categories = new Category
            {
                Id = 1,
                ParentCategoryId = 0,
                Name = "Category1" 
            };

            _categoryService.Setup(service => service.AddCategory(categories));

            var response = _controller.Post(categories);

            // Assert
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult);

            var json = JsonConvert.SerializeObject(okResult.Value);
            var jObject = JObject.Parse(json);

            Assert.AreEqual(categories.Name, jObject["Category"]["Name"].ToString());
        }


        [Test]
        public void UpdateCategory_ReturnsUpdatedResult()
        {
            int id = 1;
            var categories = new Category
            {
                Id = 1,
                ParentCategoryId = 0,
                Name = "Category1"
            };

            _categoryService.Setup(service => service.UpdateCategory(categories));

            var response = _controller.Put(id, categories);

            // Assert
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult);

            var json = JsonConvert.SerializeObject(okResult.Value);
            var jObject = JObject.Parse(json);

            Assert.AreEqual(categories.Name, jObject["Category"]["Name"].ToString());
        }

        [Test]
        public void DeleteCategory_ReturnsOk_WhenIsDeleted()
        {
            int id = 1;
            var categories = new Category
            {
                Id = 1,
                ParentCategoryId = 0,
                Name = "Category1"
            };

            _categoryService.Setup(service => service.GetCategoryById(id)).Returns(categories);
            _categoryService.Setup(service => service.DeleteCategory(categories)).Verifiable();

            var response = _controller.Delete(id);

            // Assert
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult);

            var responseString = okResult.Value.ToString();
            Assert.IsTrue(responseString.Contains("Category deleted successfully"));
        }

    }
}