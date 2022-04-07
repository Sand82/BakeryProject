using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;
using Bakery.Service;
using Bakery.Tests.Mock;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xunit;

namespace Bakery.Tests.Services
{
    public class BakerySeviceTests
    {

        [Fact]
        public void GetAllProductsReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var bakerySevice = new BakerySevice(data);

            var categoryList = new List<string> { "Bread", "Cookies" };

            var model = new AllProductQueryModel();                  
                        
            model.Categories = categoryList;            
            model.SearchTerm = string.Empty;
            model.Category = string.Empty;

           var result = bakerySevice.GetAllProducts(model);

            Assert.NotNull(result);
            Assert.StrictEqual(10, result.TotalProduct);           
          }

        [Fact]
        public void GetAllProductsReturnCorrectResultWhenAddCategoryTerms()
        {
            using var data = DatabaseMock.Instance;

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var bakerySevice = new BakerySevice(data);

            var categoryList = new List<string> { "Bread", "Cookies" };

            var model = new AllProductQueryModel();
                      
            model.Categories = categoryList;
            model.SearchTerm = string.Empty;
            model.Category = "Bread";

            var result = bakerySevice.GetAllProducts(model);

            Assert.NotNull(result);
            Assert.StrictEqual(10, result.TotalProduct);
        }

        [Fact]
        public void GetAllProductsReturnCorrectResultWhenUseSerchTearms()
        {
            using var data = DatabaseMock.Instance;

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var bakerySevice = new BakerySevice(data);

            var categoryList = new List<string> { "Bread", "Cookies" };

            var model = new AllProductQueryModel();
            
            model.Categories = categoryList;
            model.SearchTerm = string.Empty;
            model.Category = string.Empty;
            model.SearchTerm = "Bread2";

            var result = bakerySevice.GetAllProducts(model);

            Assert.NotNull(result);
            Assert.StrictEqual(1, result.TotalProduct);
        }

        [Fact]
        public void CreatProductReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;            

            var bakerySevice = new BakerySevice(data);

            var model = new BakeryFormModel();

            var categoy = GetCategory();

            var ingredients = GetIngredientAddFormModel();

            model.Name = "Bread";
            model.Description = "Great bread";
            model.CategoryId = categoy.Id;
            model.ImageUrl = "Bread.png";
            model.Price = 3.20m;
            model.Ingredients = ingredients;

            var result = bakerySevice.CreateProduct(model);

            Assert.NotNull(result);            
        }

        [Fact]
        public void EditProductReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var bakerySevice = new BakerySevice(data);
            
            var category = GetCategory();

            var ingredients = GetIngredientAddFormModel();
            
            var currModel = new ProductDetailsServiceModel();
            currModel.Id = 1;
            currModel.Name = "Bread1";            
            currModel.Description = "Bread Bread Bread Bread Bread.";
            currModel.CategoryId = category.Id;            
            currModel.ImageUrl = "nqma1.png";
            currModel.Price = 3.20m;
            currModel.Ingredients = ingredients;

            var result = bakerySevice.EditProduct(1);

            var obj1Str = JsonConvert.SerializeObject(result);
            var obj2Str = JsonConvert.SerializeObject(currModel);

            Assert.Equal(obj1Str, obj2Str);           
        }

        private List<Product> ProductsCollection()
        {
            var ingredients = GetIngredients();

            var category = GetCategory();

            var products = new List<Product>();

            for (int i = 1; i <= 10; i++)
            {
                var product = new Product {
                    Id = i,
                    Name = $"Bread{i}",
                    IsDelete = false,
                    Description = "Bread Bread Bread Bread Bread.",
                    Price = 3.20m, ImageUrl = $"nqma{i}.png",
                    Ingredients = ingredients,
                    Category = category,
                    CategoryId = category.Id };

                products.Add(product);

            }

            for (int i = 11; i <= 15; i++)
            {
                var product = new Product { Id = i, Name = $"Bread{i}", IsDelete = true, Description = "Bread Bread Bread Bread Bread.", Price = 3.20m, ImageUrl = $"nqma{i}.png", Ingredients = ingredients };

                products.Add(product);

            }

            return products;
        }

        private Category GetCategory()
        {
            return new Category { Id = 1, Name = "Bread" };
        }

        private ICollection<Ingredient> GetIngredients()
        {
            var ingredients = new HashSet<Ingredient>(){new Ingredient { Id = 1, Name = "Ingredient1"  },
                new Ingredient { Id = 2, Name = "Ingredient2" },
                new Ingredient { Id = 3, Name = "Ingredient3" } };

            return ingredients;
        }

        private ICollection<IngredientAddFormModel> GetIngredientAddFormModel()
        {
            var ingredients = new HashSet<IngredientAddFormModel>(){new IngredientAddFormModel { Name = "Ingredient1"  },
                new IngredientAddFormModel {  Name = "Ingredient2" },
                new IngredientAddFormModel {  Name = "Ingredient3" } };

            return ingredients;
        }
    }
}
