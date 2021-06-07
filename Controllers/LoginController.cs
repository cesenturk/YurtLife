using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YurtLife.Models;

namespace YurtLife.Controllers
{
    public class LoginController : BaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Title = "Giriş Yapınız";
            return View();
        }
        [HttpPost]
        public ActionResult Login(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var cus = Db.Customer.SingleOrDefault(d => d.EmailAdress.Equals(customer.EmailAdress) && d.Password.Equals(customer.Password));
                if (cus != null)
                {
                    Session["loginkey"] = cus;
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.Title = "Giriş Yapınız";
            return View(customer);
        }
    }
}