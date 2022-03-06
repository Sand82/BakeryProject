using Bakery.Data;
using Bakery.Models.Bakeries;
using Bakery.Models.Items;
using Microsoft.AspNetCore.Mvc;

namespace Bakery.Controllers
{
    public class ItemController : Controller
    {
        private readonly BackeryDbContext data;

        public ItemController(BackeryDbContext data)
        {
            this.data = data;
        }
        public IActionResult Details(int id)
        {
            var productData = this.data.Products.FirstOrDefault(p => p.Id == id);

            if (productData == null)
            {
                return NotFound();
            }

            var ingridientData = this.data
                .ProductsIngredients
                .Where(ip => ip.ProductId == id)
                .Select(i => new IngredientAddFormModel
                {
                    Name = i.Ingredient.Name,
                })
                .ToList();

            var product = new DetailsViewModel
            {
                Name = productData.Name,
                ImageUrl = productData.ImageUrl,
                Price = productData.Price.ToString("f2"),
                Description = productData.Description,
                Ingridients = ingridientData,
            };

            return View(product);
        }
    }
}
