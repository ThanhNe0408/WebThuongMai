using BanDoOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Security.Claims;

namespace BanDoOnline.Controllers
{
    public class HomeController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public ActionResult Index(int? page)
        {
            // Lấy giỏ hàng và đặt vào ViewBag
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuongGioHang = lstGioHang.Sum(item => item.SoLuong);
            ViewBag.WelcomeMessage = TempData["WelcomeMessage"] as string;

            int pageSize = 8; // Điều chỉnh số sản phẩm trên mỗi trang
            int pageNumber = (page ?? 1);

            var products = db.SANPHAMs.ToList().ToPagedList(pageNumber, pageSize);

            return View(products);
        }

        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ChiTietSanPham(int ProductID)
        {
            var mainProduct = db.SANPHAMs.SingleOrDefault(s => s.MaSP == ProductID);

            if (mainProduct == null)
            {
                return HttpNotFound();
            }

            // Lấy các sản phẩm liên quan (bạn cần định nghĩa logic cho điều này)
            var relatedProducts = GetRelatedProducts(ProductID);

            ViewBag.RelatedProducts = relatedProducts;
            return View(mainProduct);
        }

        // Bạn cần triển khai một phương thức để lấy các sản phẩm liên quan dựa trên ID sản phẩm chính
        private List<SANPHAM> GetRelatedProducts(int productId)
        {
            // Triển khai logic để lấy các sản phẩm liên quan dựa trên ID sản phẩm chính
            List<SANPHAM> relatedProductsList = new List<SANPHAM>();

            // Ví dụ: Lấy 5 sản phẩm ngẫu nhiên từ cơ sở dữ liệu
            relatedProductsList = db.SANPHAMs.Where(s => s.MaSP != productId).OrderBy(x => Guid.NewGuid()).Take(5).ToList();

            return relatedProductsList;
        }


        public ActionResult TimKiem(string searchString)
        {
            // Lấy tất cả sản phẩm từ cơ sở dữ liệu
            var sanpham = from sp in db.SANPHAMs select sp;

            // Kiểm tra xem chuỗi tìm kiếm có rỗng hay không
            if (!String.IsNullOrEmpty(searchString))
            {
                // Chuyển chuỗi tìm kiếm về dạng chữ thường
                searchString = searchString.ToLower();

                // Lọc các sản phẩm có tên chứa chuỗi tìm kiếm
                sanpham = sanpham.Where(s => s.TenSP.ToLower().Contains(searchString));
            }

            // Trả về view với danh sách sản phẩm sau khi tìm kiếm
            return View("TimKiem", sanpham.ToList());
        }


        public ActionResult DoanhMucPartial()
        {
            var danhMuc = from c in db.DANHMUCs select c;
            return PartialView(danhMuc);
        }
        public ActionResult tentheodanhmuc(int? page, int MaDM)
        {
            ViewBag.MaSP = MaDM;
            int iSize = 8;
            int iPageNumber = (page ?? 1);

            var SanPham = from s in db.SANPHAMs where s.MaDM == MaDM select s;
            return View(SanPham.ToPagedList(iPageNumber, iSize));
        }

        public ActionResult LoginLogout()
        {
            return PartialView("LoginLogoutPartial");
        }
        public ActionResult NavbarPatial()
        {
            return PartialView();
        }
        public ActionResult CategoriesPartial()
        {
            return PartialView();
        }
        public ActionResult FeaturedPartial()
        {
            return PartialView();
        }
        public ActionResult TopbarPartial()
        {
            return PartialView();
        }
       
        public ActionResult FooterPartial()
        {
            return PartialView();
        }
    }
}