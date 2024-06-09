using IS220_PROJECT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IS220_PROJECT.Controllers
{
    public class ProductController : Controller
    {
        private readonly dbFrameContext _context;

        public ProductController(dbFrameContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(x => x.ProductId == id);
            var cat = _context.Categories.FirstOrDefault(x => x.CatId == product.CatId);
            ViewData["CurrentCat"] = cat.CatName;
            if(product == null)
                return RedirectToAction("Index");
            return View(product);
        }
    }
}
