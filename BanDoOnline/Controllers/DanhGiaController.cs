using BanDoOnline.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoOnline.Controllers
{
    public class DanhGiaController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        // GET: DanhGia
        public ActionResult Index()
        {
            // Retrieve the list of reviews
            var danhGiaList = db.DANHGIAs.ToList();

            // Pass the list to the view
            return View(danhGiaList);
        }


        [HttpPost]
        public ActionResult Index(DANHGIA newReview)
        {
            if (ModelState.IsValid)
            {
                // Save the new review to the database
                db.DANHGIAs.InsertOnSubmit(newReview);
                db.SubmitChanges();
            }

            // Retrieve the updated list of reviews
            var danhGiaList = db.DANHGIAs.ToList();

            // Return the partial view with the updated list
            return PartialView("_Testimonials", danhGiaList);
        }
        public ActionResult Create()
        {
            try
            {
                // Lấy danh sách sản phẩm từ cơ sở dữ liệu
                var productList = db.SANPHAMs.ToList();

                // Lấy thông tin sản phẩm đầu tiên trong danh sách
                var defaultProduct = productList.FirstOrDefault();

                // Chuyển đổi danh sách sản phẩm thành danh sách SelectListItem
                var productSelectList = productList.Select(p => new SelectListItem
                {
                    Value = p.MaSP.ToString(),
                    Text = p.TenSP
                });

                // Gán danh sách sản phẩm và thông tin sản phẩm mặc định vào ViewData
                ViewData["MaSP"] = productSelectList;
                ViewData["DefaultProduct"] = defaultProduct;

                // Truyền model DANHGIA mới với thông tin sản phẩm mặc định vào View
                return View(new DANHGIA { MaSP = defaultProduct?.MaSP });
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi lấy danh sách sản phẩm
                // Log ex if needed
                ModelState.AddModelError("", "Error retrieving product list. Please try again.");
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DANHGIA danhGia)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Thêm đánh giá mới vào cơ sở dữ liệu
                    db.DANHGIAs.InsertOnSubmit(danhGia);
                    db.SubmitChanges();

                    // Chuyển hướng đến hành động Index và chuyển thông tin sản phẩm
                    return RedirectToAction("Index", new { id = danhGia.MaSP });
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi lưu vào cơ sở dữ liệu
                // Log ex if needed
                ModelState.AddModelError("", "Error saving review to the database. Please try again.");
            }

            // Nếu có lỗi hoặc khi lưu vào cơ sở dữ liệu, lấy lại danh sách sản phẩm và trả về View
            try
            {
                var productList = db.SANPHAMs.ToList();
                ViewData["MaSP"] = productList.Select(p => new SelectListItem
                {
                    Value = p.MaSP.ToString(),
                    Text = p.TenSP
                });
                ViewData["DefaultProduct"] = productList.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi lấy danh sách sản phẩm
                // Log ex if needed
                ModelState.AddModelError("", "Error retrieving product list. Please try again.");
            }

            return View(danhGia);
        }



    }
}