using EcommercePerifericos.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcommercePerifericos.Controllers
{
    public class HomeController : Controller
    {
        private Models.DB.AleshopBdContext dbContext;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , Models.DB.AleshopBdContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
