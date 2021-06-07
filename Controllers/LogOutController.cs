using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YurtLife.Controllers
{
    public class LogOutController : BaseController
    {
        
        public ActionResult LogOut()
        {
            
            {
                Session.Clear();
                Session.Abandon();
                return RedirectToAction("Index","Home");
            }
            
        }
    }
}