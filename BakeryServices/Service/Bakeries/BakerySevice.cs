using Bakery.Data;
using Bakery.Data.Models;
using Bakery.Models.Bakeries;
using Bakery.Models.Bakery;
using Bakery.Models.Items;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Bakery.Service.Bakeries
{
    public class BakerySevice : IBakerySevice
    {
        private readonly BakeryDbContext data;
         

        public BakerySevice(BakeryDbContext data)
        {
            this.data = data;            
        }        

        public AllProductQueryModel GetAllProducts(AllProductQueryModel query, string path)
        {          

            Task.Run(() =>
            {               

                var productQuery = this.data.Products.AsQueryable();

                CreateSerilizationFile(path);

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
        
        public ProductDetailsServiceModel EditProduct(int id)
        {
            ProductDetailsServiceModel? product = null;

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

        public void Edit( ProductDetailsServiceModel product, Product productDate)
        {
            Task.Run(() =>
            {             
                productDate.Name = product.Name;
                productDate.Description = product.Description;
                productDate.Price = product.Price;
                productDate.ImageUrl = product.ImageUrl;
                productDate.CategoryId = product.CategoryId;

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();
        }

        public NamePriceDataModel CreateNamePriceModel(int id)
        {
            NamePriceDataModel? model = null;

            Task.Run(() =>
            {
                model = this.data.Products
                .Where(x => x.Id == id)
                .Select(p => new NamePriceDataModel
                {
                    Name = p.Name,
                    Price = p.Price.ToString()
                })
                .FirstOrDefault();               

            }).GetAwaiter().GetResult();

            return model;
        }

        public void Delete(Product product)
        {
            Task.Run(() =>
            {
                product.IsDelete = true;

                this.data.SaveChanges();

            }).GetAwaiter().GetResult();
        }

        public IEnumerable<BakryCategoryViewModel> GetBakeryCategories()
        {
            IEnumerable<BakryCategoryViewModel>? categories = null;

            Task.Run(() =>
            {
                 categories = this.data.
                 Categories.
                 Select(c => new BakryCategoryViewModel
                 {
                     Id = c.Id,
                     Name = c.Name,
                 })
                 .ToList();

            }).GetAwaiter().GetResult();

            return categories;
        }

        public Product FindById(int id)
        {
            var product = new Product();

            Task.Run(() =>
            {
                product = this.data.Products.Find(id);

            }).GetAwaiter().GetResult();

            return product;
        }

        public void AddProduct(Product product, string path)
        {
            Task.Run(() =>
            {
                this.data.Products.Add(product);

                this.data.SaveChanges();               

                SerializeToJason(path);

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

        private void CreateSerilizationFile(string path)
        {           

            if (!File.Exists(path))
            {
                SerializeToJason(path);
            }
        }

        private void SerializeToJason(string path)
        {
            
            var products = data
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
                    Ingredients = p.Ingredients.Select( i => new  
                    { 
                        Name = i.Name,                        
                    })
                    .ToList()                    
                })
                .ToList();

            var result = JsonConvert.SerializeObject(products, Formatting.Indented);

            File.WriteAllText(path, result); 
            
        }        
        
    }
}
