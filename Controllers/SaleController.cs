using ePayment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YurtLife.Models;

namespace YurtLife.Controllers
{
    public class SaleController : BaseController
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
        public ActionResult Sale(int id) 
        {
            /*
            Bir yurda veya aparta ön kayıt işlemi. Yalnızca taslak halinde.
            Bankalar ile bir bağlantı söz konusu değil. 
            Mid-term dosyasında belirtildiği gibi Future Work olarak, tamamlanmak üzere ileri bir tarihe bırakılmış durumda.
            */
            var product = Db.Product.SingleOrDefault(d => d.Id.Equals(id));
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            return View(new UserCardInfo { Product = product });
        }
        [HttpPost]
        public ActionResult Sale(UserCardInfo cardInfo)
        {
            var product = Db.Product.SingleOrDefault(d => d.Id == cardInfo.Id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                cardInfo.Product = product;
                return View(cardInfo);
            }
            ViewBag.Result = "";
            var payment = new cc5payment
            {
                host = "host",
                name = "",
                password = "",
                clientid = "",
                orderresult = 0,
                cardnumber = cardInfo.CardNumber,
                expmonth = cardInfo.ExpMonth.ToString(),
                expyear = cardInfo.ExpYear.ToString(),
                cv2 = cardInfo.SecurityNumber,
                currency = "949",
                chargetype = "Auth",
                ip = Request.ServerVariables["REMOTE_ADDR"],
                subtotal = product.Fiyat.ToString(),
                taksit = "1"
            };

            string bankaSonuc = payment.processorder();
            string bankaHata = payment.errmsg;
            string bankaOid = payment.oid;
            string bankaAppr = payment.appr;
            string bankaProv = payment.code;

            if (bankaSonuc == "1")
            {
                if (bankaAppr == "Approved")
                {
                    ViewBag.Result = "Ödeme işlemi başarıyla gerçekleşti.";
                }
                else if (bankaAppr == "Declined")
                {
                    ViewBag.Result = "Banka işlemi iptal etti.";
                }
                else
                {
                    ViewBag.Result = "Banka ile iletişim Kurulamadı.";
                }
            }
            var sale = new Sale /*Yeni Ön Kayıt*/
            {
                CustomerId = CurrentCustomer.Id,
                ProductId = cardInfo.Id,
                SaleState = (short)SaleState.Check
            };
            if (!Db.Sale.Any(d => d.CustomerId == CurrentCustomer.Id))
                /*
                Kullanıcı daha önce bir yurda veya aparta ön kayıt yaptırmış ise,
                bu kaydı iptal etmeden tekrar bir yurda veya aparta kayıt olamaz.
                */
            {
                Db.Sale.InsertOnSubmit(sale);
                var Reserved = Db.Reservation.SingleOrDefault(d => d.CustomerId == CurrentCustomer.Id && d.ProductId == product.Id);
                if (Reserved != null)
                    Db.Reservation.DeleteOnSubmit(Reserved);
                Db.SubmitChanges();
                return RedirectToAction("Index", "My");
            }
            else
            {
                /*
                Bu koşulda sitenin hata ve uyarı mesajı vermesi istendi ancak başarılı olunamadı.
                Sonuç olarak bu çabanın yerini, hiçbir işlem gerçekleştirmeden kullanıcının kendi hesap sayfasına yönlendirilmesi aldı.
                Söz konusu hata mesajları olduğunda sitenin geliştirilmesi konusunda hemfikiriz.
                */
                return RedirectToAction("Index", "My");
            }


        }
        public ActionResult DeleteSale(int? id) /*Ön kaydı iptal etme*/

        
        {
            /* 
             Future Work olarak; SaleState konusunda, yalnızca yurt veya apart müdürlüğünden yanıt beklenirken
             iptal edilebilmesi fikri hakim. İşlem tamamlanıp ücret karttan çekildikten sonra karardan vazgeçilirse
             bu durum müşteri ile yurt arası diyalogla çözülmelidir. Öte yandan, projenin bu versiyonunda ön kaydı silme
             işlemi de ödeme işlemi gibi bir taslak aşamasından fazlası değildir.
            */
            if (id.HasValue)
            {
                var Sold = Db.Sale.SingleOrDefault(d => d.ProductId == id && d.CustomerId == CurrentCustomer.Id);
                if (Sold != null)
                {
                    Db.Sale.DeleteOnSubmit(Sold);
                    Db.SubmitChanges();
                }

            }
            Session["loginkey"] = Db.Customer.Single(d => d.Id == CurrentCustomer.Id);
            return RedirectToAction("Index", "My");
        }
    }
}