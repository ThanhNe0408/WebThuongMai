using BanDoOnline.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace BanDoOnline.Controllers
{
    public class ThongTinCNController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        [HttpGet]
        public ActionResult GioiThieu()
        {
            if (Session["TaiKhoan"] != null)
            {
                KHACHHANG currentUser = Session["TaiKhoan"] as KHACHHANG;

                // Retrieve the latest data from the database
                var updatedUser = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == currentUser.MaKH);

                // Update the session with the latest data
                Session["TaiKhoan"] = updatedUser;

                return View(updatedUser);
            }
            else
            {
                return RedirectToAction("DangNhap", "User");
            }
        }





        public ActionResult Details(int id)
        {
            var khachHang = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (khachHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachHang);
        }



        public ActionResult Edit(int id)
        {
            var khachHang = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (khachHang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khachHang);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(KHACHHANG updatedKhachHang)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the existing record from the database
                var existingKhachHang = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == updatedKhachHang.MaKH);

                if (existingKhachHang == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }

                // Update the existing record with the changes
                existingKhachHang.HoTen = updatedKhachHang.HoTen;
                existingKhachHang.MatKhau = updatedKhachHang.MatKhau;
                existingKhachHang.Email = updatedKhachHang.Email;
                existingKhachHang.DiaChi = updatedKhachHang.DiaChi;
                existingKhachHang.DienThoai = updatedKhachHang.DienThoai;
                existingKhachHang.NgaySinh = updatedKhachHang.NgaySinh;

                // Additional validation and logic for editing goes here

                // Save changes to the database
                db.SubmitChanges();

                // Set a success message in TempData
                TempData["EditSuccess"] = true;

                return RedirectToAction("GioiThieu");
            }

            return View(updatedKhachHang);
        }

    }
}
