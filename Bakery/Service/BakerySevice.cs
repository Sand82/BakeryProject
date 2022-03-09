using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;

namespace Bakery.Service
{
    public class BakerySevice : IBakerySevice
    {
        private readonly BackeryDbContext data;

        public BakerySevice(BackeryDbContext data)
        {
            this.data = data;
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

        public Product CreateProduct(BakeryAddFormModel formProduct)
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

            return product;
        }

        public AuthorViewModel GetAuthorInfo()
        {
            var authorInfo = this.data.Authors.FirstOrDefault();           

            var author = new AuthorViewModel
            {
                Id = authorInfo.Id,
                FirstName = authorInfo.FirstName,
                LastName = authorInfo.LastName,
                Description = authorInfo.Description,
                ImageUrl = authorInfo.ImageUrl,
            };

            return author;
        }

        public BakeryEditFormModel FindById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
