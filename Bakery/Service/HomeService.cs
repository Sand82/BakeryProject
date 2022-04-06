using Bakery.Data;
using Bakery.Models.Bakeries;
using Bakery.Models.Home;

namespace Bakery.Service
{
    public class HomeService : IHomeService
    {

        private readonly BakeryDbContext data;

        public HomeService(BakeryDbContext data)
        {
            this.data = data;
        }

        public CountViewModel GetIndex()
        {
            CountViewModel countPlusProductModel = new CountViewModel();

            Task.Run(() =>
            {
                var products = data
                .Products
                .Where(p => p.IsDelete == false)
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

                countPlusProductModel = new CountViewModel
                {
                    AllProductViewModel = products,
                    ProductCount = data.Products.Count(),
                    IngredientCount = data.Ingredients.Distinct().Count()
                };

            }).GetAwaiter().GetResult();           

            return countPlusProductModel;
        }
    }
}
