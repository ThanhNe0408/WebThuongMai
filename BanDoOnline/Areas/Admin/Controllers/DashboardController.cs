using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoOnline.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Admin/Daskboard
        public ActionResult Index()
        {
            return View();
        }
    }
}