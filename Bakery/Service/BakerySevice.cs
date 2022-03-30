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

        public BakerySevice(BackeryDbContext data)
        {
            this.data = data;            
        }

        public AllProductQueryModel GetAllProducts(AllProductQueryModel query)
        {           

            Task.Run(() => 
            {
               var productQuery = this.data.Products.AsQueryable();

                if (!string.IsNullOrWhiteSpace(query.Category))
                {
                    productQuery = productQuery
                        .Where(p => p.Category.Name == query.Category);
                }

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
                    .Where(p => p.IsDelete == false)
                    .Skip((query.CurrentPage - 1) * AllProductQueryModel.ProductPerPage)
                    .Take(AllProductQueryModel.ProductPerPage)
                    .Select(p => new AllProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price.ToString("f2"),
                        ImageUrl = p.ImageUrl,
                        Description = p.Description,
                        Category = p.Category.Name
                    })
                    .ToList();

                query.Categories = AddCategories();

                query.TotalProduct = totalProducts;
                query.Products = products;
                               

            }).GetAwaiter().GetResult();

            return query;
        }

        public void CreateProduct(BakeryFormModel formProduct)
        {        
            Task.Run(() => 
            {
                var product = new Product
                {
                    Name = formProduct.Name,
                    Description = formProduct.Description,
                    ImageUrl = formProduct.ImageUrl,
                    Price = formProduct.Price,
                    CategoryId = formProduct.CategoryId
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

            }).GetAwaiter().GetResult();        
          
        }

        public ProductDetailsServiceModel EditProduct(int id)
        {
            ProductDetailsServiceModel? product = new ProductDetailsServiceModel();

            Task.Run(() => 
            {
                product = this.data
               .Products
               .Where(p => p.Id == id)
               .Select(p => new ProductDetailsServiceModel
               {
                   Id = p.Id,
                   Name = p.Name,
                   Description = p.Description,
                   Price = p.Price,
                   ImageUrl = p.ImageUrl,
                   CategoryId = p.CategoryId,
                   Ingredients = p.Ingredients.Select(i => new IngredientAddFormModel
                   {
                       Name = i.Name
                   })
                   .ToList()
               })
               .FirstOrDefault();

            }).GetAwaiter().GetResult();
          
            return product;
        }

        public void Edit(int id, ProductDetailsServiceModel product)
        {

            Task.Run(() =>
            {
                var productDate = FindById(id);

                productDate.Name = product.Name;
                productDate.Description = product.Description;
                productDate.Price = product.Price;
                productDate.ImageUrl = product.ImageUrl;
                productDate.CategoryId = product.CategoryId;

                //var ingredients = new List<Ingredient>();

                //foreach (var ingredient in product.Ingredients)
                //{
                //    var curredntIngredient = this.data
                //        .Ingredients
                //        .FirstOrDefault(i => i.Name == ingredient.Name);

                //    if (curredntIngredient == null)
                //    {
                //        curredntIngredient = new Ingredient
                //        {
                //            Name = ingredient.Name,
                //        };
                //    }

                //    ingredients.Add(curredntIngredient);
                //}

                //productDate.Ingredients = ingredients;

                this.data.SaveChanges();
            });            
        }

        public void Delete(Product product)
        {
            Task.Run (() => 
            {
                product.IsDelete = true;

                data.SaveChanges();

            }).GetAwaiter().GetResult();            
        }

        public IEnumerable<BakryCategoryViewModel> GetBakeryCategories()
        {
            var categories = this.data.
                Categories.
                Select(c => new BakryCategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToList();

            return categories;
        }

        public Product FindById(int id)
        {
            var product = new Product();

            Task.Run(() => 
            {
                this.data.Products.Find(id);

            }).GetAwaiter().GetResult();

            return product;
        }

        private void AddProduct(Product product)
        {
            Task.Run(() => 
            {
                this.data.Products.Add(product);

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();            
        }

        private List<string> AddCategories()
        {
            var category = new List<string>();

            Task.Run(() => 
            {
                category = this.data
               .Categories
               .Select(p => p.Name)
               .Distinct()
               .OrderBy(c => c)
               .ToList();

            }).GetAwaiter().GetResult();             

            return category;
        }        
    }
}
