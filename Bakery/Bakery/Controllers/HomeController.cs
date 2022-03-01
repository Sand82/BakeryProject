using Bakery.Models;
using Bakery.Models.Home;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bakery.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostEnvironment env;
        public HomeController(IHostEnvironment env)
        {
            this.env = env;
        }

        public IActionResult Index()
        {
            var pathOne = env.ContentRootFileProvider.GetFileInfo("wwwroot/FirstPic.jpg")?.PhysicalPath;
            var pathSecond = env.ContentRootFileProvider.GetFileInfo("SecondPic.jpg")?.PhysicalPath;
            var pathThird = env.ContentRootFileProvider.GetFileInfo("ThirdPic.jpg")?.PhysicalPath;

            var indexModel = new IndexViewModel
            {
                FirstPicture = pathOne,
                SecondPicture = pathSecond,
                ThirdPicture = pathThird,
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