

namespace YurtLife.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using YurtLife.Models;
    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            ViewBag.Title = "Anasayfa";
            return View(new HomeVm

            {
                ProductList = Db.Product.ToList()
            });
        }
    }

    
}