using Bakery.Data;
using Bakery.Models.Bakeries;
using Bakery.Models.Home;

namespace Bakery.Service
{
    public class HomeService : IHomeService
    {

        private readonly BackeryDbContext data;

        public HomeService(BackeryDbContext data)
        {
            this.data = data;
        }

        public CountViewModel GetIndex()
        {
            var products = data
                .Products
                .OrderByDescending(x => x.Id)
                .Select(p => new AllProductViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price.ToString("f2"),
                    ImageUrl = p.ImageUrl,
                    Description = p.Description,
                    Category = p.Category.Name
                })
                .Take(4)
                .ToList();

            var countPlusProductModel = new CountViewModel
            {
                AllProductViewModel = products,
                ProductCount = data.Products.Count(),
                IngredientCount = data.Ingredients.Count()
            };

            return countPlusProductModel;
        }
    }
}
