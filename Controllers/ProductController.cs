
using System;
using System.Linq;
using System.Web.Mvc;
using YurtLife.Models;

namespace YurtLife.Controllers
{
    public class ProductController : BaseController
    {
        
        public ActionResult Index(string id, DefaultFilter filter)
        {
            /*
            Yurtların ve apartların tamamının listesi. 
            Projenin bu kısmının varlığı, gerekliliği ve işlevi ile ilgili grubumuzda görüş ayrılıkları olmasına karşın, son halinin böyle kalmasına karar verildi
            */
            ViewBag.Title = "Yurtlar";
            var list = Db.Product.AsQueryable();
            if (!string.IsNullOrEmpty(id))
            {
                list = list.Where(d => d.Category.SeoText.Equals(id));
                ViewBag.CategoryName = Db.Category.Single(d => d.SeoText.Equals(id)).Text;
            }
            list = AddFilter(filter, list);
            return View(list.ToList());
        }
        public ActionResult Detail(string id)
        {
            /*
             Bir yurt veya apartın detaylı bilgisi.
             Seçilmek istenen yurt veya apart veritabanında bulunamadıysa hata vermesi istendi.
            */
            var product = Db.Product.SingleOrDefault(d => d.Seo.Equals(id));
            if (product == null)
            {
                throw new Exception("Yurt Bulunamadı");
            }
            ViewBag.Title = product.YurtAdı;
            return View(product);
        }
        
    }
}

