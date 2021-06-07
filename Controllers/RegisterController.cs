using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YurtLife.Models;

namespace YurtLife.Controllers
{
    public class RegisterController : BaseController
    {

        [HttpGet]
        public ActionResult Create()
        {
            /*
            Halihazırda giriş yapmış ve hesabından henüz çıkış yapmamış bir kullanıcının,
            register özelliğini kullanmak isterse öncelikle otomatik olarak çıkış işleminin yapılması,
            sonrasında register sayfasına yönlendirilmesinin daha makul olduğunda mutabık kalındı.
            */
            var customer = (Session["loginkey"] as Customer);
            if(customer == null)
            {
                return View();
            }
            else
            {
                Session.Clear();
                Session.Abandon();
                return View();
            }
            
        }
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            
            if (ModelState.IsValid)
            {
                Db.Customer.InsertOnSubmit(customer);
                Db.SubmitChanges();
                return RedirectToAction("Login", "Login");
            }
            return View(customer);
        }
    }
}