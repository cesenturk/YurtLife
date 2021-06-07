namespace YurtLife.Controllers
    
{   using System.Linq;
    using System.Web.Mvc;
    using YurtLife.Models;
    public class BaseController : Controller

    {
        public Customer CurrentCustomer;
        public DbDataContext Db = new DbDataContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ViewBag.CategoryList = Db.Category.OrderBy(d => d.OrderNumber).ToList();
            var customer = (Session["loginkey"] as Customer);
            CurrentCustomer = customer;
        }
        public static IQueryable<Models.Product> AddFilter(DefaultFilter filter, IQueryable<Models.Product> list)
        {
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.SearchString))
                {
                    list = list.Where(d => d.YurtAdı.Contains(filter.SearchString) || d.Kategori.Contains(filter.SearchString) || d.Adres.Contains(filter.SearchString));

                }
                if (filter.SortDir == SortDir.Asc)
                {
                    if (filter.Sort == ProductColumnEnum.YurtAdı)
                        list = list.OrderBy(d => d.YurtAdı);
                    if (filter.Sort == ProductColumnEnum.Fiyat)
                        list = list.OrderBy(d => d.Fiyat);
                    if (filter.Sort == ProductColumnEnum.OdaKapasite)
                        list = list.OrderBy(d => d.OdaKapasite);
                }
                else
                {
                    if (filter.Sort == ProductColumnEnum.YurtAdı)
                        list = list.OrderByDescending(d => d.YurtAdı);
                    if (filter.Sort == ProductColumnEnum.Fiyat)
                        list = list.OrderByDescending(d => d.Fiyat);
                    if (filter.Sort == ProductColumnEnum.OdaKapasite)
                        list = list.OrderByDescending(d => d.OdaKapasite);
                }

            }

            return list;
        }
    }
}