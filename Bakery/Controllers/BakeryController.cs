using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;
using Bakery.Service;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class BakeryController : Controller
    {
        private readonly IBakerySevice bakerySevice;
        private readonly BackeryDbContext data;

        public BakeryController(IBakerySevice bakerySevice, BackeryDbContext data)
        {
            this.bakerySevice = bakerySevice;
            this.data = data;
        }

        public IActionResult All([FromQuery]AllProductQueryModel query )
        {
            query = bakerySevice.GetAllProducts(query);

            return View(query);
        }

        public IActionResult Add()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Add(BakeryAddFormModel formProduct)
        {
            
            if (!ModelState.IsValid) 
            {
                return View();
            }

            var product = bakerySevice.CreateProduct(formProduct);

            this.data.Products.Add(product);

            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult About()
        {
            var author = bakerySevice.GetAuthorInfo();

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }        
    }
}
