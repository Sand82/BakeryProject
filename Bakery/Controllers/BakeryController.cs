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
        private readonly IAuthorService authorService;
        private readonly BackeryDbContext data;

        public BakeryController(IBakerySevice bakerySevice,IAuthorService authorService, BackeryDbContext data)
        {
            this.bakerySevice = bakerySevice;          
            this.authorService = authorService;
            this.data = data;
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
            var formProduct = new BakeryFormModel{ Categories = bakerySevice.GetBakeryCategories()};

            return View(formProduct);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(BakeryFormModel formProduct)
        {
            
            if (!this.data.Categories.Any(c => c.Id == formProduct.CategoryId))
            {
                this.ModelState.AddModelError(nameof(formProduct.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                formProduct.Categories = bakerySevice.GetBakeryCategories();

                return View(formProduct);
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

            product.Categories = bakerySevice.GetBakeryCategories();            

            return View(product);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, ProductDetailsServiceModel product)
        {

            if (!ModelState.IsValid)
            {
                product.Categories = bakerySevice.GetBakeryCategories();

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
