using Microsoft.AspNetCore.Mvc;

namespace EcommercePerifericos.Controllers
{
    public class ProductController : Controller
    {
        private Models.DB.AleshopBdContext dbContext;
        public ProductController(Models.DB.AleshopBdContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProductDetail()
        {
            return View();
        }
    }
}
