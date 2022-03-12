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

        public IActionResult All([FromQuery]AllProductQueryModel query )
        {
            query = bakerySevice.GetAllProducts(query);

            return View(query);
        }

        [Authorize]
        public IActionResult Add()
        {
            var userId = User.GetId();

            var author = authorService.IsAuthor(userId);

            if (!author)
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(BakeryAddFormModel formProduct)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bakerySevice.CreateProduct(formProduct);            

            return RedirectToAction("Index", "Home");
        }       
    }
}
