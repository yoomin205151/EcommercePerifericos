using Microsoft.AspNetCore.Mvc;

namespace EcommercePerifericos.Controllers
{
    public class CartController : Controller
    {
        private Models.DB.AleshopBdContext dbContext;
        public CartController(Models.DB.AleshopBdContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
