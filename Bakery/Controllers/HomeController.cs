using Bakery.Models;
using Bakery.Models.Home;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bakery.Controllers
{
    public class HomeController : Controller
    {     
        public IActionResult Index()
        {
            var indexModel = new IndexViewModel
            {
                FirstPicture = "./wwwroot/FirstPic.jpg",
                SecondPicture = "./wwwroot/SecondPic.jpg",
                ThirdPicture = "./wwwroot/ThirdPic.jpg",
            };

            return View(indexModel);
        }        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}