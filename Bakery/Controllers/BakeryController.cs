using Bakery.Data;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;
using Bakery.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;

namespace Bakery.Controllers
{
    public class BakeryController : Controller
    {
        private readonly IBakerySevice bakerySevice;
        private readonly BackeryDbContext data;
        private readonly IAuthorService authorService;

        public BakeryController(IBakerySevice bakerySevice, BackeryDbContext data, IAuthorService authorService)
        {
            this.bakerySevice = bakerySevice;
            this.data = data;
            this.authorService = authorService;
        }

        public IActionResult All([FromQuery] AllProductQueryModel query)
        {
            var userId = User.GetId();

            query.IsAuthor = authorService.IsAuthor(userId);

            query = bakerySevice.GetAllProducts(query);

            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {
            var author = AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(BakeryFormModel formProduct)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var author = AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }

            bakerySevice.CreateProduct(formProduct);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var author = AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }

            var product = bakerySevice.EditProduct(id);

            return View(new ProductDetailsServiceModel
            {
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
            });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, ProductDetailsServiceModel product)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var author = AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }

            bakerySevice.Edit(id, product);

            return RedirectToAction("All", "Bakery");
        }

        private bool AuthorValidation()
        {
            bool isAuthor = false;

            var userId = User.GetId();

            var author = authorService.IsAuthor(userId);

            var admin = User.IsAdmin();

            if (author || admin)
            {
                isAuthor = true;
            }
            
            return isAuthor;
        }
    }
}
