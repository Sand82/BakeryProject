using Bakery.Data;
using Bakery.Models;
using Bakery.Models.Home;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bakery.Controllers
{
    public class HomeController : Controller
    {
        private readonly BackeryDbContext data;

        public HomeController(BackeryDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            var products = data
                .Products
                .OrderByDescending(x => x.Id)
                .Select(p => new IndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price.ToString("f2"),
                    ImageUrl = p.ImageUrl,
                })
                .Take(4)
                .ToList();            

            return View(products);
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}