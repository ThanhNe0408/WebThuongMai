using BanDoOnline.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BanDoOnline.Areas.Admin.Controllers
{
    public class DonHangController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        // GET: Admin/DonHang
        public ActionResult Index(int? page)
        {
            int iPageNum = page ?? 1;
            int iPageSize = 10;
            var dhList = db.DONDATHANGs.OrderBy(n => n.MaDonHang).ToList();
            return View(dhList.ToPagedList(iPageNum, iPageSize));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var donHang = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);

            if (donHang != null)
            {
                return PartialView("_DetailsPartial", donHang);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var donHang = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);

            if (donHang != null)
            {
                return PartialView("_EditPartial", donHang);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DONDATHANG updatedDonHang)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingDonHang = db.DONDATHANGs.FirstOrDefault(n => n.MaDonHang == updatedDonHang.MaDonHang);

                    if (existingDonHang != null)
                    {
                        // Update properties of existingDonHang with the values from updatedDonHang
                        existingDonHang.DaThanhToan = updatedDonHang.DaThanhToan;
                        existingDonHang.TinhTrangGiaoHang = updatedDonHang.TinhTrangGiaoHang;
                        existingDonHang.NgayDat = updatedDonHang.NgayDat;
                        existingDonHang.NgayGiao = updatedDonHang.NgayGiao;
                        existingDonHang.MaKH = updatedDonHang.MaKH;

                        db.SubmitChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                else
                {
                    return View(updatedDonHang);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return View(updatedDonHang);
            }
        }



        [HttpGet]
        public ActionResult Delete(int id)
        {
            var donHang = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);

            if (donHang != null)
            {
                return View(donHang);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var donHang = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);

            if (donHang != null)
            {
                db.DONDATHANGs.DeleteOnSubmit(donHang);
                db.SubmitChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            // You may want to initialize any necessary data for creating a new order
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DONDATHANG newDonHang)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.DONDATHANGs.InsertOnSubmit(newDonHang);
                    db.SubmitChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(newDonHang);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return View(newDonHang);
            }
        }
    }
}
