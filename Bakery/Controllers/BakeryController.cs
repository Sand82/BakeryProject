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

        public IActionResult All([FromQuery]AllProductQueryModel query )
        {
            var productQuery = this.data.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                productQuery = productQuery
                    .Where(p =>
                    p.Name.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    p.Description.Contains(query.SearchTerm.ToLower()));
            }

            if (query.Sorting == BakiesSorting.Name)
            {
                productQuery = productQuery.OrderBy(p => p.Name);
            }
            else if(query.Sorting == BakiesSorting.Price)
            {
                productQuery = productQuery.OrderByDescending(p => p.Price);
            }
            else
            {
                productQuery = productQuery.OrderByDescending(p => p.Id);
            }

            var totalProducts = productQuery.Count();

            var products = productQuery     
                .Skip((query.CurrentPage -1) * AllProductQueryModel.ProductPerPage)
                .Take(AllProductQueryModel.ProductPerPage)
                .Select(p => new AllProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,                    
                    Price = p.Price.ToString("f2"),
                    ImageUrl = p.ImageUrl,
                })
                .ToList();

            query.TotalProduct = totalProducts;
            query.Products = products;
                        
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

            var product = new Product
            {
                Name = formProduct.Name,
                Description = formProduct.Description,
                ImageUrl = formProduct.ImageUrl,
                Price = formProduct.Price,
            };


            foreach (var ingredient in formProduct.Ingredients)
            {
                var curredntIngredient = this.data
                    .Ingredients
                    .FirstOrDefault(i => i.Name == ingredient.Name);

                if (curredntIngredient == null)
                {
                    curredntIngredient = new Ingredient
                    {
                        Name = ingredient.Name,
                    };
                }
                product.Ingredients.Add(curredntIngredient);                
            }

            this.data.Products.Add(product);

            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        
    }
}
