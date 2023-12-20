using Microsoft.AspNetCore.Mvc;

namespace EcommercePerifericos.Controllers
{
    public class favoriteController : Controller
    {
        private Models.DB.AleshopBdContext dbContext;
        public favoriteController(Models.DB.AleshopBdContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
