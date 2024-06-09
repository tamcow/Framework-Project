using Microsoft.AspNetCore.Mvc;
using IS220_PROJECT.Models;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using Microsoft.AspNetCore.Authorization;

namespace IS220_PROJECT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly dbFrameContext _context;
        private List<Post1> posts = new List<Post1>();

        public HomeController(ILogger<HomeController> logger, dbFrameContext context)
        {
            _logger = logger;
            _context = context;
            JObject postsJson = Utils.Utils.readApi("./apis/post.json");
            //List<Post> posts = new List<Post>();
            foreach (var post in postsJson)
            {
                var temp = JObject.Parse(post.Value.ToString());
                List<string> imgs = new List<string>();
                foreach(var item in temp["Thumb"])
                {
                    imgs.Add(item.First.ToString());
                }

                Post1 _post = new Post1 {PostId = int.Parse(post.Key) - 1, Title = (string)temp["Title"], Contents = (string)temp["Contents"], Thumb = imgs, Tags = (string)temp["Tags"], CreateDate = (DateTime)temp["CreateDate"], Author = (string)temp["Author"], IsHot = (bool)temp["isHot"], IsNewfeed = (bool)temp["isNewfeed"] };
                posts.Add(_post);
            }
        }

        public IActionResult Index()
        {
            var lsProduct = _context.Products.AsEnumerable().ToList();
            var lsBest = _context.Products.Where(p => p.BestSellers == true).ToList();
            ViewBag.lsBest = lsBest;
            return View(lsProduct);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult News()
        {
            return View(posts);
        }

        public IActionResult SinglePost(int id)
        {
            ViewBag.imgs = posts[id].Thumb;
            return View(posts[id]);
        }
        public IActionResult About()
        {
            return View();
        }
        
        public IActionResult Laptop(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utils.Utils.PAGE_SIZE;
            List<Product> lsProducts = _context.Products.AsNoTracking().Where(p => p.CatId == 1).ToList();
            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);

            //ViewBag.laptops = models;
            return PartialView(models);
        }

        public IActionResult PC(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utils.Utils.PAGE_SIZE;
            List<Product> lsProducts = _context.Products.AsNoTracking().Where(p => p.CatId == 18).ToList();
            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);

            //ViewBag.laptops = models;
            return PartialView(models);
        }
        public IActionResult Monitor(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utils.Utils.PAGE_SIZE;
            List<Product> lsProducts = _context.Products.AsNoTracking().Where(p => p.CatId == 2).ToList();
            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);

            //ViewBag.laptops = models;
            return PartialView(models);
        }
        public IActionResult OtherDevices(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = Utils.Utils.PAGE_SIZE;
            List<Product> lsProducts = _context.Products.AsNoTracking().Where(p => p.CatId != 18 && p.CatId != 2 && p.CatId != 1).ToList();
            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);

            //ViewBag.laptops = models;
            return PartialView(models);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}