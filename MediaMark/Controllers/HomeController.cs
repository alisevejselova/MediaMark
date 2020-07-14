using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediaMark.Models;

namespace MediaMark.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MediaMarkContext  _context;

        public HomeController(MediaMarkContext context,ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Index()
        {


            ViewBag.CategoryList = _context.Category;
            //najnovite prvi 
            ViewBag.LastProducts = _context.Products.OrderByDescending(a => a.ProductID).Skip(0).Take(12).ToList();
        
            
            return View();
        }
        public ActionResult Kategori(int id)
        {
            
            ViewBag.KategoriListesi = _context.Category.ToList();
            ViewBag.Kategori = _context.Category.Find(id);
            return View(_context.Products.Where(a => a.RefCateogryID == id).OrderBy(a => a.ProductName).ToList());
        }
        public ActionResult Produkt(int id)
        {
            ViewBag.KategoriListesi = _context.Category.ToList();
            return View(_context.Products.Find(id));
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
