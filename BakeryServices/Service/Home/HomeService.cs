using BakeryData;
using BakeryServices.Models.Bakeries;
using BakeryServices.Models.Home;
using Microsoft.EntityFrameworkCore;

namespace BakeryServices.Service.Home
{
    public class HomeService : IHomeService
    {

        private readonly BakeryDbContext data;

        public HomeService(BakeryDbContext data)
        {
            this.data = data;
        }

        public async Task<CountViewModel> GetIndex()
        {
            CountViewModel countPlusProductModel = new CountViewModel();

            var products = await data
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
            .ToListAsync();

            countPlusProductModel = new CountViewModel
            {
                AllProductViewModel = products,
                ProductCount = data.Products.Where(p => p.IsDelete == false).Count(),
                IngredientCount = data.Ingredients.Distinct().Count()
            };

            return countPlusProductModel;
        }
    }
}
