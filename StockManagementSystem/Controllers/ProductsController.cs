using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockManagementSystem.Models;
using System.Net;
using System.Net.Mail;


namespace StockManagementSystem.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(String search = "")
        {
            StockDatabaseEntities1 db = new StockDatabaseEntities1();
            ViewBag.search = search;
            List<Product> products = db.Products.Where(temp => temp.ItemName.Contains(search)).ToList();


            return View(products);
        }

        public ActionResult Report(long id)
        {
            StockDatabaseEntities1 db = new StockDatabaseEntities1();
            Product stock = db.Products.Where(temp => temp.ProductID == id).FirstOrDefault();


            return View(stock);
        }

        [HttpGet]
        public ActionResult Create()
        {

            StockDatabaseEntities1 db = new StockDatabaseEntities1();

            ViewBag.Invoices = db.Invoices.ToList();
            ViewBag.Orders = db.Orders.ToList();

            return View();
        }

        [HttpPost]
        public ActionResult Create(Product p)
        {

            if (Request.Files.Count >= 1)
            {
                var file = Request.Files[0];
                var imgBytes = new Byte[file.ContentLength];
                file.InputStream.Read(imgBytes, 0, file.ContentLength);
                var base64String = Convert.ToBase64String(imgBytes, 0, imgBytes.Length);
               
            }


            StockDatabaseEntities1 db = new StockDatabaseEntities1();
            
            ViewBag.Invoices = db.Invoices.ToList();
            ViewBag.Orders = db.Orders.ToList();
            db.Products.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            StockDatabaseEntities1 db = new StockDatabaseEntities1();
            Product existingProduct = db.Products.Where(temp => temp.ProductID == id).FirstOrDefault();
            ViewBag.Invoices = db.Invoices.ToList();
            ViewBag.Orders = db.Orders.ToList();


            return View(existingProduct);
        }
        [HttpPost]
        public ActionResult Edit(Product p)
        {

            StockDatabaseEntities1 db = new StockDatabaseEntities1();
            Product existingProduct = db.Products.Where(temp => temp.ProductID == p.ProductID).FirstOrDefault();
            existingProduct.ItemName = p.ItemName;
            existingProduct.Quantity = p.Quantity;
            existingProduct.Price = p.Price;
            existingProduct.InvoiceID = p.InvoiceID;
            existingProduct.OrderID = p.OrderID;
           
            db.SaveChanges();
            return RedirectToAction("Index", "Products");


           
        }

        [HttpGet]
        public ActionResult delete(long id)
        {

            StockDatabaseEntities1 db = new StockDatabaseEntities1();
            Product p = db.Products.Where(temp => temp.ProductID == id).FirstOrDefault();
            ViewBag.Invoices = db.Invoices.ToList();
            ViewBag.Orders = db.Orders.ToList();

            return View(p);
        }

        [HttpPost]
        public ActionResult delete(long id, Product p)
        {
            StockDatabaseEntities1 db = new StockDatabaseEntities1();
            Product product = db.Products.Where(temp => temp.ProductID == id).FirstOrDefault();
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpGet]
        public ActionResult email()
        {
            StockDatabaseEntities1 db = new StockDatabaseEntities1();
            ViewBag.Emails = db.Emails.ToList();
            return View();
        }
       [HttpPost]
        public ActionResult email(StockManagementSystem.Models.Email model)
        {

            MailMessage msge = new MailMessage("victorenyanga9@gmail.com", model.To);
            msge.Subject = model.Subject;
            msge.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("victorenyanga9@gmail.com", "gmailpassword");

            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(msge);
            ViewBag.Message = "mail has been sent succesfully!";


            return RedirectToAction("index");
        }


    }
}