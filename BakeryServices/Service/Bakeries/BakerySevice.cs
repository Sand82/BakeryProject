using BakeryData;
using BakeryData.Models;
using BakeryServices.Models.Bakeries;
using BakeryServices.Models.Items;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BakeryServices.Service.Bakeries
{
    public class BakerySevice : IBakerySevice
    {
        private readonly BakeryDbContext data;

        public BakerySevice(BakeryDbContext data)
        {
            this.data = data;
        }

        public async Task<AllProductQueryModel> GetAllProducts(AllProductQueryModel query, string path)
        {

            var productQuery = this.data.Products.AsQueryable();

            await CreateSerilizationFile(path);

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

            var totalProducts = productQuery.Where(p => p.IsDelete == false).Count();

            var products = await productQuery
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
                .ToListAsync();

            query.Categories = await AddCategories();

            query.TotalProduct = totalProducts;
            query.Products = products;

            return query;
        }

        public Product CreateProduct(BakeryFormModel formProduct)
        {
            Product? product = null;

            Task.Run(() =>
            {
                product = new Product
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

            }).GetAwaiter().GetResult();

            return product;
        }

        public async Task<ProductDetailsServiceModel> EditProduct(int id)
        {
            var product = await this.data
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
            .FirstOrDefaultAsync();

            return product;
        }

        public async Task Edit(ProductDetailsServiceModel product, Product productDate)
        {
            productDate.Name = product.Name;
            productDate.Description = product.Description;
            productDate.Price = product.Price;
            productDate.ImageUrl = product.ImageUrl;
            productDate.CategoryId = product.CategoryId;

            await this.data.SaveChangesAsync();
        }

        public async Task<NamePriceDataModel> CreateNamePriceModel(int id)
        {
            var model = await this.data.Products
            .Where(x => x.Id == id)
            .Select(p => new NamePriceDataModel
            {
                Name = p.Name,
                Price = p.Price.ToString()
            })
            .FirstOrDefaultAsync();

            return model;
        }

        public async Task Delete(Product product)
        {
            product.IsDelete = true;

            await this.data.SaveChangesAsync();
        }

        public async Task<IEnumerable<BakryCategoryViewModel>> GetBakeryCategories()
        {
            var categories = await this.data.
            Categories.
            Select(c => new BakryCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name,
            })
            .ToListAsync();

            return categories;
        }

        public async Task<Product> FindById(int id)
        {
            var product = await this.data.Products.FindAsync(id);

            return product;
        }

        public async Task AddProduct(Product product, string path)
        {
            await this.data.Products.AddAsync(product);

            await this.data.SaveChangesAsync();

            await SerializeToJason(path);
        }

        public async Task<bool> CheckCategory(int categoryId)
        {
            var isExist = await this.data.Categories
                .AnyAsync(c => c.Id == categoryId);

            return isExist;
        }

        private async Task<List<string>> AddCategories()
        {
            var category = await this.data
            .Categories
            .Select(p => p.Name)
            .Distinct()
            .OrderBy(c => c)
            .ToListAsync();

            return category;
        }

        private async Task CreateSerilizationFile(string path)
        {

            if (!File.Exists(path))
            {
                await SerializeToJason(path);
            }
        }

        private async Task SerializeToJason(string path)
        {
            var products = await data
                .Products
                .Include(p => p.Ingredients)
                .Select(p => new
                {
                    Name = p.Name,
                    Category = p.Category.Name,
                    Price = p.Price,
                    Description = p.Description,
                    IsDelete = false,
                    ImageUrl = p.ImageUrl,
                    CategoryId = p.CategoryId,
                    Ingredients = p.Ingredients.Select(i => new
                    {
                        Name = i.Name,
                    })
                    .ToList()
                })
                .ToListAsync();

            var result = JsonConvert.SerializeObject(products, Formatting.Indented);

            await File.WriteAllTextAsync(path, result);

        }
    }
}
