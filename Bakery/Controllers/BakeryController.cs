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

        public IActionResult All(string searchTerm, BakiesSorting sorting )
        {
            var carsQuery = this.data.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                carsQuery = carsQuery
                    .Where(p =>
                    p.Name.ToLower().Contains(searchTerm.ToLower()) ||
                    p.Description.Contains(searchTerm.ToLower()));
            }

            var products = carsQuery                
                .OrderByDescending(x => x.Price)
                .Select(p => new AllProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,                    
                    Price = p.Price.ToString("f2"),
                    ImageUrl = p.ImageUrl,
                })
                .ToList();
                        
            return View(new AllProductQueryModel
            {
                Products = products,
                SearchTerm = searchTerm,
            });
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
