using Bakery.Data;
using Bakery.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using static Bakery.WebConstants;

namespace Bakery.Infrastructure
{
    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder PrepareDatabase(
           this IApplicationBuilder app)
        {
            var scolpedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scolpedServices.ServiceProvider;

            var data = serviceProvider.GetRequiredService<BakeryDbContext>();

            var webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

            data.Database.Migrate();

            SeedCategories(data);

            SeedAdministrator(serviceProvider);

            SeedAuthor(data);

            SeedProducts(data, webHostEnvironment);           

            return app;
        }
               
        private static void SeedAdministrator(IServiceProvider service)
        {
            var userMager = service.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);

                    var author = new IdentityUser
                    {
                        Email = "vqra@abv.bg",
                        UserName = "vqra@abv.bg",
                    };

                    await userMager.CreateAsync(author, "123456");

                    await userMager.AddToRoleAsync(author, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedCategories(BakeryDbContext data)
        {
            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category { Name = "Breds" },
                new Category { Name = "Cookies" },
                new Category { Name = "Sweets " },
                new Category { Name = "Specialty" },
                new Category { Name = "Cakes" },
            });

            data.SaveChanges();
        }

        private static void SeedAuthor(BakeryDbContext data)
        {
            if (data.Authors.Any())
            {
                return;
            }

            data.Authors.Add(new Author
            {
                FirstName = "Vqra",
                LastName = "Hristova",
                Description = "Great baker. Always baking with the hart.",
                ImageUrl = "https://thatbreadlady.com/wp-content/uploads/2020/10/that-bread-lady-about-me-683x1024.jpg"

            });

            data.SaveChanges();
        }

        private static void SeedProducts(BakeryDbContext data, IWebHostEnvironment webHostEnvironment)
        {
            if (data.Products.Any())
            {
                return;
            }

            var file = $"{webHostEnvironment.WebRootPath}\\products.json";

            var productsDTO = JsonConvert.DeserializeObject<ICollection<SeedingProductModel>>(File.ReadAllText(file));

            CreateProduct(productsDTO, data);
        }

        private static void CreateProduct(ICollection<SeedingProductModel> productsDTO, BakeryDbContext data)
        {

            foreach (var product in productsDTO)
            {
                var currProduct = new Product
                {
                    Name = product.Name,
                    Price = product.Price,
                    Description = product.Description,
                    ImageUrl = product.ImageUrl,
                };

                var category = data.Categories.Where(c => c.Name == product.Category).FirstOrDefault();

                currProduct.CategoryId = category.Id;

                foreach (var ingredient in product.Ingredients)
                {
                    var currIngredient = data.Ingredients.FirstOrDefault(i => i.Name == ingredient.Name);

                    if (currIngredient == null)
                    {
                        currIngredient = new Ingredient
                        {
                            Name = ingredient.Name,
                        };
                    }

                    currProduct.Ingredients.Add(currIngredient);
                }

                AddInDatabase(currProduct, data);
            }
        }

        private static void AddInDatabase(Product currProduct, BakeryDbContext data)
        {
            data.Products.Add(currProduct);

            data.SaveChanges();
        }
    }
}
