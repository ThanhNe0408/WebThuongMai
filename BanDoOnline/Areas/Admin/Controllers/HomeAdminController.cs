using BanDoOnline.Models;
using System.Web.Mvc;

namespace BanDoOnline.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string name, string pass)
        {
            mapTKAdmin map = new mapTKAdmin();
            var user = map.TimKiem(name, pass);

            // Check if login is successful
            if (user != null)
            {
                // Save the username and full name to the session
                Session["Taikhoan"] = name;
                Session["HoVaten"] = user.HoVaTen;

                // Redirect to the dashboard
                return RedirectToAction("Index", "HomeAdmin", new { area = "Admin" });
            }

            // If login fails, display an error message
            ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng";
            return View();
        }

    }
}
