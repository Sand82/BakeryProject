using Bakery.Models;
using Bakery.Service.Home;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bakery.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        public IActionResult Index()
        {
            var countPlusProductModel = homeService.GetIndex();

            return View(countPlusProductModel);
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}