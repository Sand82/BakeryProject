using Bakery.Controllers;
using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;
using Bakery.Service;
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

            var product = ProductsCollection();

            data.Products.AddRange(product);

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.All(new AllProductQueryModel());

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.NotNull(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<AllProductQueryModel>(model);

            var expect = product.Where(x => x.IsDelete == false).ToList();

            Assert.Equal(expect.Count, indexViewModel.TotalProduct);
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

            data.Authors.Add(new Author
            {
                Id = 1,
                FirstName = "Sand",
                LastName = "Stef",
                Description = "Test test test",
                ImageUrl = "nqma.png",
                AuthorId = "vqra@abv.bg"
            });

            var product = ProductsCollection();

            data.Products.AddRange(product);

            var category = new Category { Id =1, Name = "Breads" };            

            data.SaveChanges();

            var controller = CreateClaimsPrincipal(data);

            var result = controller.Edit(1);

            var viewResult = Assert.IsType<ViewResult>(result);

            Assert.NotNull(result);

            var model = viewResult.Model;

            var indexViewModel = Assert.IsType<ProductDetailsServiceModel>(model);
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


        private BakeryController CreateClaimsPrincipal(BakeryDbContext data)
        {
            var bakeryService = new BakerySevice(data);

            var authorService = new AuthorService(data, null);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.NameIdentifier, "vqra@abv.bg"),
                new Claim(ClaimTypes.Name, "vqra@abv.bg")

           }, "TestAuthentication"));

            var controller = new BakeryController(bakeryService, authorService, data);

            controller.ControllerContext = new ControllerContext();

            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

            return controller;
        }
    }    
}
