using Bakery.Controllers;
using BakeryData;
using BakeryData.Models;
using BakeryServices.Models.Bakeries;
using BakeryServices.Service.Authors;
using BakeryServices.Service.Bakeries;
using Bakery.Tests.Mock;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

using static Bakery.Tests.GlobalMethods.TestService;

namespace Bakery.Tests.Controllers
{
    public class BakeryControllerTests
    {

        [Fact]
        public void AllActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var products = ProductsCollection();

            data.Products.AddRange(products);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);          
                        
            var result = controller.All(new AllProductQueryModel());

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.NotNull(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<AllProductQueryModel>(model);

            var expected = products.Where(x => x.IsDelete == false).ToList();

            Assert.Equal(expected.Count, indexViewModel.TotalProduct);
        }

        [Fact]
        public void AddActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Add(new BakeryFormModel());

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.NotNull(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<BakeryFormModel>(model);
        }

        [Fact]
        public void AddPostActionShouldReturnBadRequestWhenUserIsNotAdmin()
        {
            using var data = DatabaseMock.Instance;

            var category = new Category { Name = "Breads" };

            data.Categories.Add(category);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var formModel = new BakeryFormModel()
            {
                Name = "Bread whit butter and 3 tipes flour",
                Price = 3.20m,
                Description = "Good bread",
                ImageUrl = "nqma.png",
                CategoryId = 1,
            };

            var result = controller.Add(formModel);

            var viewResult = Assert.IsType<BadRequestResult>(result);

            Assert.NotNull(result);
        }

        [Fact]
        public void EditActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var author = CreateAuthor();

            data.Authors.Add(author);

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Edit(1);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.NotNull(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<ProductDetailsServiceModel>(model);
        }

        [Fact]
        public void EditPostActionShouldReturnCorectResult()
        {
            using var data = DatabaseMock.Instance;

            var author = CreateAuthor();

            data.Authors.Add(author);

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var modelForm = new ProductDetailsServiceModel
            {
                Id = 1,
                Name = "Bread",
                Price = 3.00m,
                Description = "Test test test",
                ImageUrl = "noimages.png",
                CategoryId = 1,
                Categories = new List<BakryCategoryViewModel>() { new BakryCategoryViewModel { Id = 1, Name = "Bread" } },
                Ingredients = new List<IngredientAddFormModel>()
                {
                    new IngredientAddFormModel { Name = "salt" },
                    new IngredientAddFormModel { Name = "fluor"}
                }
            };

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Edit(1, modelForm);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.NotNull(result);

            var model = data.Products.FirstOrDefault(p => p.Id == 1);

            Assert.Equal(modelForm.Name, model.Name);
            Assert.Equal(modelForm.Price, model.Price);
            Assert.Equal(modelForm.Description, model.Description);
            Assert.Equal(modelForm.ImageUrl, model.ImageUrl);
        }

        [Fact]
        public void EditPostActionShouldReturnBadRequestResultIfModelIsNotValid()
        {
            using var data = DatabaseMock.Instance;

            var author = CreateAuthor();

            data.Authors.Add(author);

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var modelForm = new ProductDetailsServiceModel
            {
                Id = 10,
                Name = "",
                Price = 3.00m,
                Description = "Test test tes",
                ImageUrl = "",
                CategoryId = 1,
                Categories = new List<BakryCategoryViewModel>() { new BakryCategoryViewModel { Id = 1, Name = "Bread" } },
                Ingredients = new List<IngredientAddFormModel>()
                {
                    new IngredientAddFormModel { Name = "salt" },
                    new IngredientAddFormModel { Name = "fluor"}
                }
            };

            var controller = CreateClaimsPrincipal(data);

            controller.ViewData.ModelState.AddModelError("Key", "ErrorMessage");

            var result = controller.Edit(1, modelForm);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.NotNull(result);

            Assert.True(viewResult.ViewData.ModelState.Count > 0);

        }


        //[Fact]
        //public void AddPostActionShouldReturnCorectResultWhenAuthorIsLoged()
        //{
        //    using var data = DatabaseMock.Instance;

        //    data.Authors.Add(new Author
        //    {
        //        Id = 1,
        //        FirstName = "Sand",
        //        LastName = "Stef",
        //        Description = "Test test test",
        //        ImageUrl = "nqma.png",
        //        AuthorId = "vqra@abv.bg"
        //    });

        //    var category = new Category { Name = "Breads" };

        //    data.Categories.Add(category);

        //    data.SaveChanges();

        //    var ingredient = new List<IngredientAddFormModel>()
        //    {
        //        new IngredientAddFormModel
        //        {
        //            Name = "butter",
        //        },
        //        new IngredientAddFormModel
        //        {
        //            Name = "sugar"
        //        }
        //    };

        //    var controller = CreateClaimsPrincipal(data);

        //    var formModel = new BakeryFormModel()
        //    {
        //        Name = "Bread whit butter and 3 tipes flour",
        //        Price = 3.20m,
        //        Description = "Good bread",
        //        ImageUrl = "nqma.png",
        //        CategoryId = 1,
        //        Ingredients = ingredient,
        //    };                      

        //    var result = controller.Add(formModel);

        //    var viewResult = Assert.IsType<ViewResult>(result);

        //    Assert.NotNull(result);
        //}

        [Fact]
        public void DeleteActionShouldRemoveProduct()
        {

            using var data = DatabaseMock.Instance;

            var author = CreateAuthor();

            data.Authors.Add(author);

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Delete(1);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.NotNull(result);

            var model = data.Products.FirstOrDefault(p => p.Id == 1);            

            Assert.True(model.IsDelete == true);
        }

        [Fact]
        public void DeleteActionShouldReturnBadRequestWhenProdoctDontExist()
        {

            using var data = DatabaseMock.Instance;

            var author = CreateAuthor();

            data.Authors.Add(author);

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Delete(100);

            var viewResult = Assert.IsType<BadRequestResult>(result);

            Assert.NotNull(result);            
        }


        private BakeryController CreateClaimsPrincipal(BakeryDbContext data)
        {
            var bakeryService = new BakerySevice(data);

            var authorService = new AuthorService(data, null);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.NameIdentifier, "vqra@abv.bg"),
                new Claim(ClaimTypes.Name, "vqra@abv.bg")

           }, "TestAuthentication"));

            var controller = new BakeryController(bakeryService, authorService, null);

            controller.ControllerContext = new ControllerContext();

            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            return controller;
        }

        private Author CreateAuthor()
        {
            var author = new Author
            {
                Id = 1,
                FirstName = "Sand",
                LastName = "Stef",
                Description = "Test test test",
                ImageUrl = "nqma.png",
                AuthorId = "vqra@abv.bg"
            };

            return author;
        }
    }
}
