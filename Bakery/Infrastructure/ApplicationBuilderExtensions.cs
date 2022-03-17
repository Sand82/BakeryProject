using Bakery.Data;
using Bakery.Data.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

            var data = serviceProvider.GetRequiredService<BackeryDbContext>();

            data.Database.Migrate();

            SeedCategories(data);

            SeedAdministrator(serviceProvider);

            SeedAuthor(data);

            //SeedProducts(data);

            //SeedDayOfTheWeek(data)

            return app;
        }



        private static void SeedDayOfTheWeek(BackeryDbContext data)
        {
            //if (data.DayOfTheWeek.Any())
            //{
            //    return;
            //}

            //data.DayOfTheWeek.AddRange(new[]
            //{
            //    new DayOfTheWeek{ Name = "Monday"},
            //});
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

        private static void SeedCategories(BackeryDbContext data)
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

        private static void SeedAuthor(BackeryDbContext data)
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

        private static void SeedProducts(BackeryDbContext data)
        {
            if (data.Products.Any())
            {
                return;
            }

            var productsDTO = new List<SeedingProductModel>();

            productsDTO.AddRange(new[]
            {
                new SeedingProductModel
                {
                    ImageUrl = "https://img.taste.com.au/aourKSds/w720-h480-cfill-q80/taste/2017/12/muffin-pan-fried-rice-cups-133596-1.jpg",
                    Name = "Muffin-pan fried rice cups",
                    Description="Turn a family-favourite into delicious freezer-friendly muffins - perfect for school and work lunch boxes.",
                    Price = 4.30M,
                    Category = "Cakes",
                    Ingredients = new List<SeedingIngredientModel>()
                    {
                         new SeedingIngredientModel {Name = "brown rice" },
                         new SeedingIngredientModel { Name = "bacon"},
                         new SeedingIngredientModel { Name = "onion"},
                         new SeedingIngredientModel { Name = "frozen peas"},
                         new SeedingIngredientModel { Name = "corn"},
                         new SeedingIngredientModel { Name = "sauce"},
                         new SeedingIngredientModel { Name = "eggs"},
                    }

                },
                new SeedingProductModel
                {
                    Name = "Quick and easy pizza muffins",
                    ImageUrl = "https://img.taste.com.au/VRktMUgZ/w720-h480-cfill-q80/taste/2016/11/quick-and-easy-pizza-muffins-107044-1.jpeg",
                    Description="These quick and easy pizza muffins can be made with a variety of toppings - so there's something everyone will love!",
                    Price = 2.55M,
                    Category = "Specialty",
                    Ingredients = new List<SeedingIngredientModel>()
                    {
                       new SeedingIngredientModel {Name = "fluor"},
                       new SeedingIngredientModel {Name = "sugar"},
                       new SeedingIngredientModel {Name = "salt"},
                       new SeedingIngredientModel {Name = "extra virgin olive oil"},
                       new SeedingIngredientModel {Name = "pizza sauce"},
                       new SeedingIngredientModel {Name = "mozzarella cheese"},
                       new SeedingIngredientModel {Name = "fresh basil"},
                       new SeedingIngredientModel {Name = "tomatoes"},
                       new SeedingIngredientModel {Name = "mixed herbs"},
                    }

                },
                new SeedingProductModel
                {
                    Name = "Strawberry scone cakes",
                    ImageUrl = "https://img.taste.com.au/sc07Cz8f/w720-h480-cfill-q80/taste/2016/11/strawberry-scone-cakes-104539-1.jpeg",
                    Description="For the perfect afternoon treat, try one of these delicious strawberry scone cakes.",
                    Price = 3.30M,
                    Category = "Cakes",
                    Ingredients = new List<SeedingIngredientModel>()
                    {
                        new SeedingIngredientModel {Name = "butter"},
                        new SeedingIngredientModel {Name = "flour"},
                        new SeedingIngredientModel {Name = "baking powder "},
                        new SeedingIngredientModel {Name = "sugar"},
                        new SeedingIngredientModel {Name = "vanilla"},
                        new SeedingIngredientModel {Name = "buttermilk"},
                        new SeedingIngredientModel {Name = "strawberries"},
                        new SeedingIngredientModel {Name = "eggs"},
                        new SeedingIngredientModel {Name = "cream"},
                        new SeedingIngredientModel {Name = "sugar"},
                    }

                },
                 new SeedingProductModel
                {
                    Name = "Chunky chocolate, coconut and banana muffins",
                    ImageUrl = "https://img.taste.com.au/9XWAiVDI/w720-h480-cfill-q80/taste/2016/11/chunky-chocolate-coconut-and-banana-muffins-101038-1.jpeg",
                    Description="Indulge in these healthier banana muffins. The whole family will love them!",
                    Price = 1.70M,
                    Category = "Specialty",
                    Ingredients = new List<SeedingIngredientModel>()
                    {
                        new SeedingIngredientModel {Name = "bananas"},
                        new SeedingIngredientModel {Name = "eggs"},
                        new SeedingIngredientModel {Name = "coconut oil"},
                        new SeedingIngredientModel {Name = "baking soda"},
                        new SeedingIngredientModel {Name = "salt"},
                        new SeedingIngredientModel {Name = "chocolate"},
                    }
                },
                new SeedingProductModel
                {
                    Name = "Crispy mac and cheese muffins",
                    ImageUrl = "https://img.taste.com.au/pOxxyEq6/w720-h480-cfill-q80/taste/2016/11/crispy-mac-and-cheese-muffins-103157-1.jpeg",
                    Description="Create extra-crunchy edges with these crispy mac and cheese muffins!",
                    Price = 2.30M,
                    Category = "Specialty",
                    Ingredients = new List<SeedingIngredientModel>()
                    {
                        new SeedingIngredientModel {Name = "pasta"},
                        new SeedingIngredientModel {Name = "milk"},
                        new SeedingIngredientModel {Name = "bay leaf"},
                        new SeedingIngredientModel {Name = "black peppercorns"},
                        new SeedingIngredientModel {Name = "bacon"},
                        new SeedingIngredientModel {Name = "oil"},
                        new SeedingIngredientModel {Name = "butter"},
                        new SeedingIngredientModel {Name = "fluor"},
                        new SeedingIngredientModel {Name = "Dijon mustard"},
                        new SeedingIngredientModel {Name = "eggs"},
                        new SeedingIngredientModel {Name = "bocconcini"},
                    }
                },
                new SeedingProductModel
                {
                    Name = "Spaghetti bolognese cups",
                    ImageUrl = "https://img.taste.com.au/Ef2B2kGg/w720-h480-cfill-q80/taste/2020/01/apple-and-custard-muffins-158075-2.jpg",
                    Description="Freeze a batch of our delicious apple-custard muffins and they'll keep for up to three months. Is it afternoon tea time yet?",
                    Price = 2.30M,
                    Category = "Specialty",
                    Ingredients = new List<SeedingIngredientModel>()
                    {
                        new SeedingIngredientModel {Name = "milk"},
                        new SeedingIngredientModel {Name = "oil"},
                        new SeedingIngredientModel {Name = "apple puree"},
                        new SeedingIngredientModel {Name = "flour"},
                        new SeedingIngredientModel {Name = "sugar"},
                        new SeedingIngredientModel {Name = "cinnamon"},
                        new SeedingIngredientModel {Name = "maple syrup"},
                    }
                },
                new SeedingProductModel
                {
                    Name = "Breakfast hash brown and egg cups",
                    ImageUrl = "https://img.taste.com.au/S9LtJYEu/w720-h480-cfill-q80/taste/2016/11/breakfast-hash-brown-and-egg-cups-107032-1.jpeg",
                    Description="These fun egg, bacon and hash brown breakfast cups are a great treat for the weekend.",
                    Price = 3.20M,
                    Category = "Specialty",
                    Ingredients = new List<SeedingIngredientModel>()
                    {
                        new SeedingIngredientModel {Name = "potatoes"},
                        new SeedingIngredientModel {Name = "parmesan"},
                        new SeedingIngredientModel {Name = "bacon"},
                        new SeedingIngredientModel {Name = "chives"},
                        new SeedingIngredientModel {Name = "eggs"},
                    }
                },
            });

            CreateProduct(productsDTO, data);
        }

        private static void CreateProduct(List<SeedingProductModel> productsDTO, BackeryDbContext data)
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

        private static void AddInDatabase(Product currProduct, BackeryDbContext data)
        {
            data.Products.Add(currProduct);

            data.SaveChanges();
        }
    }
}
