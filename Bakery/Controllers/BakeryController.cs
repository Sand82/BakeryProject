using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class BakeryController : Controller
    {
        private readonly BackeryDbContext data;

        public BakeryController(BackeryDbContext data)
        {
            this.data = data;
        }

        public IActionResult All()
        {
            var products = data
                .Products
                .OrderBy(x => x.Id)
                .Select(p => new AllProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,                    
                    Price = p.Price.ToString("f2"),
                    ImageUrl = p.ImageUrl,
                })
                .ToList();
                        
            return View(products);
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

            var product = new Product
            {
                Name = formProduct.Name,
                Description = formProduct.Description,
                ImageUrl = formProduct.ImageUrl,
                Price = formProduct.Price,
            };

            this.data.Products.Add(product);

            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
    }
}
