using Bakery.Data;
using Bakery.Models.Bakeries;
using Bakery.Models.Items;

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
            var productData = this.data.Products.FirstOrDefault(p => p.Id == id);
                       
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

            return product;
        }
    }
}
