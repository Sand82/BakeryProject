using Bakery.Data;
using Bakery.Infrastructure;
using Bakery.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BackeryDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddAntiforgery(options => 
{
    options.HeaderName = "X-ANTIF-TOKEN";
});

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    //options.Lockout.MaxFailedAccessAttempts = 5;
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    //options.User.RequireUniqueEmail = true;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<BackeryDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IBakerySevice, BakerySevice>();
builder.Services.AddTransient<IHomeService, HomeService>();
builder.Services.AddTransient<IItemsService, ItemsService>();
builder.Services.AddTransient<IAuthorService, AuthorService>();
builder.Services.AddTransient<IVoteService, VoteService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IOrganizerService, OrganizerService>();

var app = builder.Build();

app.PrepareDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}

app
   .UseHttpsRedirection()
   .UseStaticFiles()
   .UseRouting()
   .UseAuthentication()
   .UseAuthorization();

app.MapControllerRoute(
    name: "Areas",
    pattern: "/{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
