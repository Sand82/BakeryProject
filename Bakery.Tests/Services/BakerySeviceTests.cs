using BakeryData;
using BakeryData.Models;
using BakeryServices.Models.Bakeries;
using BakeryServices.Models.Items;
using BakeryServices.Service.Bakeries;

using Bakery.Tests.Mock;

using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Services
{
    public class BakerySeviceTests
    {

        [Fact]
        public async Task GetAllProductsReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            AddProductsInDatabase(data);

            var bakerySevice = new BakerySevice(data);

            var categoryList = new List<string> { "Bread", "Cookies" };

            var model = new AllProductQueryModel();                  
                        
            model.Categories = categoryList;            
            model.SearchTerm = string.Empty;
            model.Category = string.Empty;

            var path = string.Empty;

            var result = await bakerySevice.GetAllProducts(model, path);

            Assert.NotNull(result);
            Assert.StrictEqual(10, result.TotalProduct);           
          }

        [Fact]
        public async Task GetAllProductsReturnCorrectResultWhenAddCategoryTerms()
        {
            using var data = DatabaseMock.Instance;

            AddProductsInDatabase(data);

            var bakerySevice = new BakerySevice(data);

            var categoryList = new List<string> { "Bread", "Cookies" };

            var model = new AllProductQueryModel();
                      
            model.Categories = categoryList;
            model.SearchTerm = string.Empty;
            model.Category = "Bread";

            var result = await bakerySevice.GetAllProducts(model, null);

            Assert.NotNull(result);
            Assert.StrictEqual(10, result.TotalProduct);
        }

        [Fact]
        public async Task GetAllProductsReturnCorrectResultWhenUseSerchTearms()
        {
            using var data = DatabaseMock.Instance;

            AddProductsInDatabase(data);

            var bakerySevice = new BakerySevice(data);

            var categoryList = new List<string> { "Bread", "Cookies" };

            var model = new AllProductQueryModel();
            
            model.Categories = categoryList;
            model.SearchTerm = string.Empty;
            model.Category = string.Empty;
            model.SearchTerm = "Bread2";

            var result = await bakerySevice.GetAllProducts(model, null);

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
        public async Task EditProductReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            AddProductsInDatabase(data);

            var bakerySevice = new BakerySevice(data);
            
            var category =  GetCategory();

            var ingredients = GetIngredientAddFormModel();
            
            var currModel = new ProductDetailsServiceModel();
            currModel.Id = 1;
            currModel.Name = "Bread1";            
            currModel.Description = "Bread Bread Bread Bread Bread.";
            currModel.CategoryId = category.Id;            
            currModel.ImageUrl = "nqma1.png";
            currModel.Price = 3.20m;
            currModel.Ingredients = ingredients;

            var result = await bakerySevice.EditProduct(1);

            var obj1Str = ConvertToJason(result);
            var obj2Str = ConvertToJason(currModel);

            Assert.Equal(obj1Str, obj2Str);           
        }

        [Fact]
        public async Task FinByIdShouldReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            AddProductsInDatabase(data);

            var bakerySevice = new BakerySevice(data);

            var model = new BakeryFormModel();            

            var result = await bakerySevice.FindById(1);

            Assert.NotNull(result);
        }

        [Fact]
        public void FinByIdShouldReturnZeroResultWithEmptyDatabase()
        {
            using var data = DatabaseMock.Instance;            

            var bakerySevice = new BakerySevice(data);

            var model = new BakeryFormModel();

            var result = bakerySevice.FindById(1);

            Assert.Null(result);
        }

        [Fact]
        public void GetBakeryCategoriesShouldReturnCorrectResult()
        {

            using var data = DatabaseMock.Instance;

            var categories = GetListOfCategories();

            data.Categories.AddRange(categories);

            data.SaveChanges();

            var bakerySevice = new BakerySevice(data);

            var result = bakerySevice.GetBakeryCategories();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetBakeryCategoriesShouldReturnEmptyResultIfNoDataOnDatabase()
        {
            using var data = DatabaseMock.Instance;            

            var bakerySevice = new BakerySevice(data);

            var result = await bakerySevice.GetBakeryCategories();

            Assert.Empty(result);
        }

        [Fact]
        public async Task EditShouldReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            AddProductsInDatabase(data);          
            
            var bakerySevice = new BakerySevice(data);

            var product = await bakerySevice.FindById(10);

            var model = new ProductDetailsServiceModel()
            {
                Name = "Basi",
                Description = "Great descriprion",
                ImageUrl = "nqma1.png",
                Price = 3.76m,
                CategoryId = 1
            };

            await bakerySevice.Edit(model, product);

            await data.SaveChangesAsync();

            var result = await bakerySevice.FindById(10);
                      
            Assert.NotNull(result);
            Assert.Equal(model.Name, result.Name);
            Assert.Equal(model.Description, result.Description);
            Assert.Equal(model.ImageUrl, result.ImageUrl);
            Assert.Equal(model.Price, result.Price);
            Assert.Equal(model.CategoryId, result.CategoryId);
        }       

        [Fact]
        public async Task CreateNamePriceModelShouldReturnCorrectResult()
        {
            using var data = DatabaseMock.Instance;

            AddProductsInDatabase(data);

            var bakerySevice = new BakerySevice(data);

            var product = bakerySevice.CreateNamePriceModel(1);

            var model = await bakerySevice.FindById(1);

            var result = new NamePriceDataModel
            {
                Name = "Bread1",
                Price = 3.20m.ToString(),
            };         
                    
            Assert.NotNull(result);
            Assert.Equal(model.Name, result.Name);
            Assert.Equal(model.Price.ToString(), result.Price);                
        }

        private void AddProductsInDatabase(BakeryDbContext data)
        {
            var product = ProductsCollection();

             data.Products.AddRange(product);

             data.SaveChanges();
        }               

        private List<Category> GetListOfCategories()
        {
            var categories = new List<Category>() 
            {
                new Category  {Id = 1, Name = "Bread" },
                new Category  {Id = 2, Name = "Sweets" },
                new Category  {Id = 3, Name = "Cookies" },               
            };

            return categories;
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
