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

            SeedProducts(data);

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

            var products = new List<Product>();

            products.AddRange(new[]
            {
                new Product
                {
                    ImageUrl = "https://img.taste.com.au/aourKSds/w720-h480-cfill-q80/taste/2017/12/muffin-pan-fried-rice-cups-133596-1.jpg",
                    Name = "Muffin-pan fried rice cups",
                    Description="Turn a family-favourite into delicious freezer-friendly muffins - perfect for school and work lunch boxes.",
                    Price = 4.30M,
                    Category = new Category { Name = "Cakes"},
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient {Name = "brown rice"},
                        new Ingredient {Name = "bacon"},
                        new Ingredient {Name = "onion"},
                        new Ingredient {Name = "frozen peas"},
                        new Ingredient {Name = "corn"},
                        new Ingredient {Name = "sauce"},
                        new Ingredient {Name = "eggs"},
                    }

                },
                new Product
                {
                    Name = "Quick and easy pizza muffins",
                    ImageUrl = "https://img.taste.com.au/VRktMUgZ/w720-h480-cfill-q80/taste/2016/11/quick-and-easy-pizza-muffins-107044-1.jpeg",
                    Description="These quick and easy pizza muffins can be made with a variety of toppings - so there's something everyone will love!",
                    Price = 2.55M,
                    Category = new Category { Name = "Specialty"},
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient {Name = "fluor"},
                        new Ingredient {Name = "sugar"},
                        new Ingredient {Name = "salt"},
                        new Ingredient {Name = "extra virgin olive oil"},
                        new Ingredient {Name = "pizza sauce"},
                        new Ingredient {Name = "mozzarella cheese"},
                        new Ingredient {Name = "fresh basil"},
                        new Ingredient {Name = "tomatoes"},
                        new Ingredient {Name = "mixed herbs"},
                    }

                },
                new Product
                {
                    Name = "Strawberry scone cakes",
                    ImageUrl = "https://img.taste.com.au/sc07Cz8f/w720-h480-cfill-q80/taste/2016/11/strawberry-scone-cakes-104539-1.jpeg",
                    Description="For the perfect afternoon treat, try one of these delicious strawberry scone cakes.",
                    Price = 3.30M,
                    Category = new Category { Name = "Cakes"},
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient {Name = "butter"},
                        new Ingredient {Name = "flour"},
                        new Ingredient {Name = "baking powder "},
                        new Ingredient {Name = "sugar"},
                        new Ingredient {Name = "vanilla"},
                        new Ingredient {Name = "buttermilk"},
                        new Ingredient {Name = "strawberries"},
                        new Ingredient {Name = "eggs"},
                        new Ingredient {Name = "cream"},
                        new Ingredient {Name = "sugar"},
                    }

                },
                 new Product
                {
                    Name = "Chunky chocolate, coconut and banana muffins",
                    ImageUrl = "https://img.taste.com.au/9XWAiVDI/w720-h480-cfill-q80/taste/2016/11/chunky-chocolate-coconut-and-banana-muffins-101038-1.jpeg",
                    Description="Indulge in these healthier banana muffins. The whole family will love them!",
                    Price = 1.70M,
                    Category = new Category { Name = "Specialty"},
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient {Name = "bananas"},
                        new Ingredient {Name = "eggs"},
                        new Ingredient {Name = "coconut oil"},
                        new Ingredient {Name = "baking soda"},
                        new Ingredient {Name = "salt"},
                        new Ingredient {Name = "chocolate"},
                    }
                },
                new Product
                {
                    Name = "Crispy mac and cheese muffins",
                    ImageUrl = "https://img.taste.com.au/pOxxyEq6/w720-h480-cfill-q80/taste/2016/11/crispy-mac-and-cheese-muffins-103157-1.jpeg",
                    Description="Create extra-crunchy edges with these crispy mac and cheese muffins!",
                    Price = 2.30M,
                    Category = new Category { Name = "Specialty"},
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient {Name = "pasta"},
                        new Ingredient {Name = "milk"},
                        new Ingredient {Name = "bay leaf"},
                        new Ingredient {Name = "black peppercorns"},
                        new Ingredient {Name = "bacon"},
                        new Ingredient {Name = "oil"},
                        new Ingredient {Name = "butter"},
                        new Ingredient {Name = "fluor"},
                        new Ingredient {Name = "Dijon mustard"},
                        new Ingredient {Name = "eggs"},
                        new Ingredient {Name = "bocconcini"},
                    }
                },
                new Product
                {
                    Name = "Spaghetti bolognese cups",
                    ImageUrl = "https://img.taste.com.au/Ef2B2kGg/w720-h480-cfill-q80/taste/2020/01/apple-and-custard-muffins-158075-2.jpg",
                    Description="Freeze a batch of our delicious apple-custard muffins and they'll keep for up to three months. Is it afternoon tea time yet?",
                    Price = 2.30M,
                    Category = new Category { Name = "Specialty"},
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient {Name = "milk"},
                        new Ingredient {Name = "oil"},
                        new Ingredient {Name = "apple puree"},
                        new Ingredient {Name = "flour"},
                        new Ingredient {Name = "sugar"},
                        new Ingredient {Name = "cinnamon"},
                        new Ingredient {Name = "maple syrup"},
                    }
                },
                new Product
                {
                    Name = "Breakfast hash brown and egg cups",
                    ImageUrl = "https://img.taste.com.au/S9LtJYEu/w720-h480-cfill-q80/taste/2016/11/breakfast-hash-brown-and-egg-cups-107032-1.jpeg",
                    Description="These fun egg, bacon and hash brown breakfast cups are a great treat for the weekend.",
                    Price = 3.20M,
                    Category = new Category { Name = "Specialty"},
                    Ingredients = new List<Ingredient>()
                    {
                        new Ingredient {Name = "potatoes"},
                        new Ingredient {Name = "parmesan"},
                        new Ingredient {Name = "bacon"},
                        new Ingredient {Name = "chives"},
                        new Ingredient {Name = "eggs"},
                    }
                },
            });

            products.SelectMany(p => p.Ingredients).Distinct();

            data.Products.AddRange(products);
                      
            data.SaveChanges();
        }
    }
}
