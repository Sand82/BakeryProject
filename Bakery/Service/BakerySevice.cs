using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;

using static Bakery.Infrastructure.ClaimsPrincipalExtensions;

namespace Bakery.Service
{
    public class BakerySevice : IBakerySevice
    {
        private readonly BackeryDbContext data;
        private readonly IAuthorService authorService;

        public BakerySevice(BackeryDbContext data, IAuthorService authorService)
        {
            this.data = data;
            this.authorService = authorService;
        }           
        
        public AllProductQueryModel GetAllProducts(AllProductQueryModel query)
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
            else if (query.Sorting == BakiesSorting.Price)
            {
                productQuery = productQuery.OrderByDescending(p => p.Price);
            }
            else
            {
                productQuery = productQuery.OrderByDescending(p => p.Id);
            }

            var totalProducts = productQuery.Count();

            var products = productQuery
                .Skip((query.CurrentPage - 1) * AllProductQueryModel.ProductPerPage)
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

            return query;
        }

        public void CreateProduct(BakeryAddFormModel formProduct)
        {
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

            AddProduct(product);
        }   
        
        private void AddProduct(Product product)
        {
            this.data.Products.Add(product);

            this.data.SaveChanges();
        }
    }
}
