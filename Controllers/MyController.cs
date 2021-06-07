using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YurtLife.Models;
using System.ComponentModel.DataAnnotations;

namespace YurtLife.Controllers
{
    public class MyController : BaseController
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /*
            Giriş yapmamış bir kullanıcı, müşteri bilgisinin kullanılmasının zorunlu olduğu alanlarla ilgili işlem yapmak istediğinde login yapmaya zorlanıyor.
            Projenin tasarlanma aşamasında SystemNullReference hataları ile sık sık karşılaştığımız ve üstesinden gelemediğimiz için böyle bir çözüme başvurduk. 
            */
            ViewBag.CategoryList = Db.Category.OrderBy(d => d.OrderNumber).ToList();
            var customer = (Session["loginkey"] as Customer);
            if (filterContext.ActionDescriptor.ActionName != "Login")
                if (customer == null)
                {
                    filterContext.Result = RedirectToAction("Login", "Login");
                    return;
                }
            CurrentCustomer = customer;
        }

        public ActionResult Index() /*Favorilerim*/
        {

            ViewBag.Title = "Rezervasyonlarım";
            ViewBag.ReservedProduct = Db.Reservation.Where(d => d.CustomerId == CurrentCustomer.Id).Select(d => d.Product).ToList();
            ViewBag.Sale = Db.Sale.Where(d => d.CustomerId == CurrentCustomer.Id).ToList();
            return View();
        }
        public ActionResult AddReservation(int? id) /*Favorilere Ekleme*/
        {
            if (id.HasValue)
            {
                if (!Db.Reservation.Any(d => d.ProductId == id && d.CustomerId == CurrentCustomer.Id))
                {
                    var ReservationItem = new Reservation
                    {
                        CustomerId = CurrentCustomer.Id,
                        ProductId = id.Value
                    };
                    Db.Reservation.InsertOnSubmit(ReservationItem);
                    Db.SubmitChanges();
                }

            }
            Session["loginkey"] = Db.Customer.Single(d => d.Id == CurrentCustomer.Id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveReservation(int? id) /*Favorilerden Çıkarma*/
        {
            if (id.HasValue)
            {
                var ReservationItem = Db.Reservation.SingleOrDefault(d => d.ProductId == id && d.CustomerId == CurrentCustomer.Id);
                if (ReservationItem != null)
                {
                    Db.Reservation.DeleteOnSubmit(ReservationItem);
                    Db.SubmitChanges();
                }

            }
            Session["loginkey"] = Db.Customer.Single(d => d.Id == CurrentCustomer.Id);
            return RedirectToAction("Index");
        }


    }
}
