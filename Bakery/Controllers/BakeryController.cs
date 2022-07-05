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
        public async Task<IActionResult> All([FromQuery] AllProductQueryModel query)
        {
            var userId = User.GetId();

            if (userId == null)
            {
                return BadRequest();
            }

            query.IsAuthor = await authorService.IsAuthor(userId);

            var path = GetJsonFilePath();

            query = await bakerySevice.GetAllProducts(query, path);

            return View(query);
        }

        [Authorize]
        public async Task<IActionResult> Add()
        {
            var author = await AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }            
            var formProduct = new BakeryFormModel{ Categories = await bakerySevice.GetBakeryCategories()};

            return View(formProduct);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(BakeryFormModel formProduct)
        {

            var categoryId = formProduct.CategoryId;

            bool isValidCategory = await bakerySevice.CheckCategory(categoryId);

            if (!isValidCategory)
            {
                this.ModelState.AddModelError(nameof(categoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                formProduct.Categories = await bakerySevice.GetBakeryCategories();

                return View(formProduct);
            }           

            var author = await AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }

            var product = bakerySevice.CreateProduct(formProduct);

            var path = GetJsonFilePath();

            await bakerySevice.AddProduct(product, path);

            this.TempData[SuccessOrder] = "Product added seccessfully.";

            return RedirectToAction("All", "Bakery");
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var author = await AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }

            var product = await bakerySevice.EditProduct(id);

            product.Categories = await bakerySevice.GetBakeryCategories();            

            return View(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductDetailsServiceModel productModel)
        {
            if (!ModelState.IsValid)
            {
                productModel.Categories = await bakerySevice.GetBakeryCategories();

                return View(productModel);
            }

            var author = await AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }

            var product = await bakerySevice.FindById(id);

            await bakerySevice.Edit(productModel, product);

            return RedirectToAction("All", "Bakery");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await AuthorValidation();

            if (!author)
            {
                return BadRequest();
            }

            var product = await bakerySevice.FindById(id);

            if (product == null )
            {
                return BadRequest();
            }

            await bakerySevice.Delete(product);

            return RedirectToAction("All", "Bakery");
        }

        private async Task<bool> AuthorValidation()
        {
            bool isAuthor = false;

            var userId = User.GetId();

            var author = await authorService.IsAuthor(userId);

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
