using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediaMark.Models;

namespace MediaMark.Controllers
{
    public class OrdersController : Controller
    {
        private readonly MediaMarkContext _context;

        public OrdersController(MediaMarkContext context)
        {
            _context = context;
        }

        // GET: Orders
      
        public ActionResult Index()
        {
            var order = _context.Order.Include( m =>m.OrderDetail).ToList();
            return View(order.ToList());

        }

        public ActionResult OrderDetailsAction(int orderId)
        {

            var orderdetail = _context.OrderDetails.Include( a =>a.Products).Where(a => a.RefOrderID == orderId).ToList();
            return View(orderdetail.ToList());
        }


        public ActionResult OrderComplete()
        {   // imam samo eden korisnik vo baza.
            int userID = 1;
            IEnumerable<Cart> cartProducts = _context.Cart.Where(a => a.RefUserID == 1).ToList();

            string ClientId = "100300000"; // Bankadan aldığınız mağaza kodu
            string Amount = cartProducts.Sum(a => a.TotalPrice).ToString(); // sepettteki ürünlerin toplam fiyatı
            string Oid = String.Format("{0:yyyyMMddHHmmss}", DateTime.Now); // sipariş id oluşturuyoruz. her sipariş için farklı olmak zorunda
            string OnayURL = "http://localhost:44307/Orders/Completed"; // Ödeme tamamlandığında bankadan verilerin geleceği url
            string HataURL = "http://localhost:44307/Orders/Error/"; // Ödeme hata verdiğinde bankadan gelen verilerin gideceği url
            string RDN = "asdf"; // hash karşılaştırması için eklenen rast gele dizedir
            string StoreKey = "123456"; // Güvenlik anahtarı bankanın sanal pos sayfasından alıyoruz


            string TransActionType = "Auth"; // bu bölüm sabit değişmiyor
            string Instalment = "";
            string HashStr = ClientId + Oid + Amount + OnayURL + HataURL + TransActionType + Instalment + RDN + StoreKey; // Hash oluşturmak için bankanın bizden istediği stringleri birleştiriyoruz

            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] HashBytes = System.Text.Encoding.GetEncoding("ISO-8859-9").GetBytes(HashStr);
            byte[] InputBytes = sha.ComputeHash(HashBytes);
            string Hash = Convert.ToBase64String(InputBytes);

            ViewBag.ClientId = ClientId;
            ViewBag.Oid = Oid;
            ViewBag.okUrl = OnayURL;
            ViewBag.failUrl = HataURL;
            ViewBag.TransActionType = TransActionType;
            ViewBag.RDN = RDN;
            ViewBag.Hash = Hash;
            ViewBag.Amount = Amount;
            ViewBag.StoreType = "3d_pay_hosting"; // Ödeme modelimiz biz buna göre anlatıyoruz 
            ViewBag.Description = "";
            ViewBag.XID = "";
            ViewBag.Lang = "tr";
            ViewBag.EMail = "destek@karayeltasarim.com";
            ViewBag.UserID = "karayelapi"; // bu id yi bankanın sanala pos ekranında biz oluşturuyoruz.
            ViewBag.PostURL = "https://entegrasyon.asseco-see.com.tr/fim/est3Dgate";


            return View();
        }


        //public ActionResult Completed()
        //{
        //    int userID = 1;

        //    Order order = new Order()
        //    {
        //        //For posted form input, use request.form
        //        FirstName = Request.Form.Get("FirstName"),
        //        LastName = Request.Form.Get("LastName"),
        //        Address = Request.Form.Get("Address"),
        //        OrderDate = DateTime.Now,
        //        IdentityNumber = Request.Form.Get("IdentityNumber"),
        //        PhoneNumber = Request.Form.Get("PhoneNumber"),
        //        RefUserID = userID
        //    };

        //    IEnumerable<Cart> cartProducts = _context.Cart.Where(a => a.RefUserID == userID).ToList();

        //    foreach (Cart cartProd in cartProducts)
        //    {
        //        OrderDetails newOrderDetails = new OrderDetails()
        //        {
        //            Quantity = cartProd.Quantity,
        //            TotalPrice = cartProd.TotalPrice,
        //            RefProductID = cartProd.RefProductID
        //        };

        //        order.OrderDetail.Add(newOrderDetails);

        //        _context.Cart.Remove(cartProd);
        //    }

        //    _context.Order.Add(order);
        //    _context.SaveChanges();

        //    return View();
        //}

        public ActionResult Error()
        {

            ViewBag.Hata = Request.Form;

            return View();
        }
        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}
