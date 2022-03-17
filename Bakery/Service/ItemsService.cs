using Bakery.Data;
using Bakery.Models.Bakeries;
using Bakery.Models.Items;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Service
{
    public class ItemsService : IItemsService
    {
        private readonly BackeryDbContext data;

        public ItemsService(BackeryDbContext data)
        {
            this.data = data;
        }

        public DetailsViewModel GetDetails(int id)
        {
            //var productData = this.data.Products.FirstOrDefault(p => p.Id == id);

            var product = this.data.
                 Products
                .Include(i => i.Ingredients)
                .Where(p => p.Id == id)
                .Select(p => new DetailsViewModel 
                { 
                   Id = p.Id,
                   Name = p.Name,
                   Price = p.Price.ToString("f2"),
                   Description = p.Description,
                   ImageUrl = p.ImageUrl,
                   Category = p.Category.Name,
                   Ingridients = p.Ingredients.Select(i => new IngredientAddFormModel
                   {
                        Name = i.Name,
                   })
                   .OrderBy(i => i.Name)
                   .ToList()

                })
                .FirstOrDefault();           

            return product;
        }
    }
}
