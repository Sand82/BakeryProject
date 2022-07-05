using Bakery.Data;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;
using Bakery.Service.Bakeries;
using Bakery.Service.Authors;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;
using static Bakery.WebConstants;

namespace Bakery.Controllers
{
    public class BakeryController : Controller
    {
        private readonly IBakerySevice bakerySevice;    
        private readonly IAuthorService authorService;
        private readonly BakeryDbContext data;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BakeryController(IBakerySevice bakerySevice,
            IAuthorService authorService,
            BakeryDbContext data,
            IWebHostEnvironment webHostEnvironment)
        {
            this.bakerySevice = bakerySevice;          
            this.authorService = authorService;
            this.data = data;
            this.webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        public IActionResult All([FromQuery] AllProductQueryModel query)
        {
            var userId = User.GetId();

            if (userId == null)
            {
                return BadRequest();
            }

            query.IsAuthor = authorService.IsAuthor(userId);

            var path = GetJsonFilePath();

            query = bakerySevice.GetAllProducts(query, path);

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

           var product = bakerySevice.CreateProduct(formProduct);

            var path = GetJsonFilePath();

            bakerySevice.AddProduct(product, path);

            this.TempData[SuccessOrder] = "Product added seccessfully.";

            return RedirectToAction("All", "Bakery");
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
        public IActionResult Edit(int id, ProductDetailsServiceModel productModel)
        {
            if (!ModelState.IsValid)
            {
                productModel.Categories = bakerySevice.GetBakeryCategories();

                return View(productModel);
            }

            var author = AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }

            var product = bakerySevice.FindById(id);

            bakerySevice.Edit(productModel, product);

            return RedirectToAction("All", "Bakery");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var author = AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }

            var product = bakerySevice.FindById(id);

            if (product == null )
            {
                return BadRequest();
            }

            bakerySevice.Delete(product);

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

        private string GetJsonFilePath()
        {
            return $"{webHostEnvironment.WebRootPath}/products.json";
        }
    }
}
