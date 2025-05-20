using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BanDoOnline.Models;
using BCrypt.Net;

namespace BanDoOnline.Controllers
{
    public class UserController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            var returnUrl = TempData["ReturnUrl"] != null
        ? TempData["ReturnUrl"].ToString()
        : null;
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection, string returnUrl)
        {

            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                // Kiểm tra đăng nhập
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN);
                if (kh != null && VerifyPassword(sMatKhau, kh.MatKhau))
                {
                    ViewBag.ThongBao = "Chúc mừng bạn đăng nhập thành công";

                    // Lưu thông tin người dùng vào Session
                    Session["TaiKhoan"] = kh;
                    Session["HoTen"] = kh.HoTen;

                    if (collection["remember"].Contains("true"))
                    {
                        // Lưu thông tin đăng nhập vào Cookies nếu người dùng chọn "Remember Me"
                        Response.Cookies["TenDN"].Value = sTenDN;
                        Response.Cookies["Matkhau"].Value = sMatKhau;
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(1);
                        Response.Cookies["Matkhau"].Expires = DateTime.Now.AddDays(1);
                    }
                    else
                    {
                        // Xóa Cookies nếu người dùng không chọn "Remember Me"
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["Matkhau"].Expires = DateTime.Now.AddDays(-1);
                    }

                    // Chuyển hướng đến returnUrl nếu có, nếu không thì chuyển đến trang mặc định
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }


        [HttpGet]
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null; // Xóa phiên đăng nhập
            return RedirectToAction("Index", "Home"); // Chuyển hướng người dùng đến trang chủ sau khi đăng xuất
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(KHACHHANG kh)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    // Kiểm tra sự tồn tại của tên đăng nhập và email
                    if (db.KHACHHANGs.Any(n => n.TaiKhoan == kh.TaiKhoan))
                    {
                        ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
                        return View(kh);
                    }

                    if (db.KHACHHANGs.Any(n => n.Email == kh.Email))
                    {
                        ViewBag.ThongBao = "Email đã được sử dụng";
                        return View(kh);
                    }

                    // Mã hóa mật khẩu trước khi lưu vào CSDL
                    kh.MatKhau = HashPassword(kh.MatKhau);

                    // Thêm khách hàng vào CSDL
                    db.KHACHHANGs.InsertOnSubmit(kh);
                    db.SubmitChanges();

                    ViewBag.ThongBao = "Đăng ký thành công. Hãy đăng nhập để trải nghiệm!";
                    return RedirectToAction("DangNhap");
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                ViewBag.ThongBao = "Đã xảy ra lỗi. Vui lòng thử lại.";
                // Ghi log ex.Message để kiểm tra lỗi
            }

            // Trả về view với model nếu có lỗi
            return View(kh);
        }

        // Hàm mã hóa mật khẩu bằng bcrypt
        public string HashPassword(string password)
        {
            // Sử dụng hàm HashPassword để mã hóa mật khẩu
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Hàm kiểm tra tính hợp lệ của mật khẩu đã được mã hóa
        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Sử dụng hàm Verify để kiểm tra mật khẩu
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }
}