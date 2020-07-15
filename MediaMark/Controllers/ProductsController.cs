using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediaMark.Models;
using System.Reflection;
using MediaMark.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MediaMark.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MediaMarkContext _context;

       
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(MediaMarkContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

       // GET: Products
       
        public ActionResult Index(string sortOrder)
        {
            ViewBag.PriceSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
 
            var products = from s in  _context.Products.Include(m => m.Category)
                           select s;
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.ProductPrice);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductPrice);
                    break;
            }
            return View(products.ToList());
        }
   


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
           
            if (products == null)
            {
                return NotFound();
            }
           ViewData["ID"] = _context.Products.Where(t => t.ProductID == id).Select(t => t.ProductID).FirstOrDefault();
          
            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["KategoriID"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryName");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductPicture Vmodel)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(Vmodel);

                Products product = new Products
                { 
                    ProductID = Vmodel.ProductID,
                    Picture = uniqueFileName,
                    ProductName = Vmodel.ProductName,
                    ProductDescription = Vmodel.ProductDescription,
                    ProductPrice = Vmodel.ProductPrice,
                    RefCateogryID = Vmodel.RefCateogryID,
                    Category=Vmodel.Category,
                    ShoppingCart=Vmodel.ShoppingCart,
                    OrderDetail=Vmodel.OrderDetail,

                };

                _context.Add(product);
                await _context.SaveChangesAsync();
                ViewData["KategoriID"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryName", product.RefCateogryID);
                return RedirectToAction(nameof(Index));
            }
            
            
            return View();
        }
        private string UploadedFile(ProductPicture model)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Picture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["KategoriID"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryName");
            ViewData["ProductID"] = products.ProductID;
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,ProductDescription,ProductPrice,RefCateogryID")] Products products)
        {
            if (id != products.ProductID)
            {
                return NotFound();
            }
          
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategoriID"] = new SelectList(_context.Set<Category>(), "CategoryId", "CategoryName", products.RefCateogryID);
            return View(products);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.Include( m => m.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["ID"] = _context.Products.Where(t => t.ProductID == id).Select(t => t.ProductID).FirstOrDefault();
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            //izbrisi ja slikata
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "images", products.Picture);
            FileInfo file = new FileInfo(path);
            if (file.Exists)//check file exsit or not
            {
                file.Delete();
            }

            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
