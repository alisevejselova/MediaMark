using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediaMark.Models;
using System.Net;

namespace MediaMark.Controllers
{
    public class CartsController : Controller
    {
        private readonly MediaMarkContext _context;

        public CartsController(MediaMarkContext context)
        {
            _context = context;
        }

        // GET: Carts


        public ActionResult AddToCart(int? quantity, int id) // quantity po default e eden
        {
            //momentalno imam samo eden korisnik vo baza ,nemam registration i login
            int userID = 2;
            Cart CartProduct = _context.Cart.FirstOrDefault(a => a.RefProductID == id && a.RefUserID == userID);
            //Cart CartProducts = _context.Cart.FirstOrDefault(a => a.RefProductID == id);
            Products product = _context.Products.Find(id);

            int price = int.Parse(product.ProductPrice);

            if (CartProduct == null) // ako vekje postoi eden product,a se dodava uste eden togas samo se zgolemuva quantity
            {
                Cart newProduct = new Cart()
                {
                    RefUserID = userID,
                    RefProductID = id,
                    Quantity = quantity ?? 1,
                    TotalPrice = (quantity ?? 1) * price,
                };
                _context.Cart.Add(newProduct);

            }
            else
            {
                CartProduct.Quantity = CartProduct.Quantity + (quantity ?? 1);
                CartProduct.TotalPrice = CartProduct.Quantity * price;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            int userID = 2;
            
            var cart = _context.Cart.Where(a => a.RefUserID == userID).Include(s => s.Products);
          //  var cart = _context.Cart.Include(s => s.Products);
            return View(cart.ToList());
        }

        public ActionResult CartUpdate(int? quantity, int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cart cart = _context.Cart.Find(id);

            if (cart == null)
            {
                return HttpNotFound();
            }

            Products product = _context.Products.Find(cart.RefProductID);

            cart.Quantity = quantity ?? 1;
            int pom = Convert.ToInt32(product.ProductPrice);
            cart.TotalPrice = cart.Quantity * Convert.ToInt32(product.ProductPrice);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private ActionResult HttpNotFound()
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int id)
        {
            Cart cart = _context.Cart.Find(id);
            _context.Cart.Remove(cart);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartID == id);
        }
    }
}
