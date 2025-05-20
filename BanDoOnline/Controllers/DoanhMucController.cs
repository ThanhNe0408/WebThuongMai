using BanDoOnline.Models;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace BanDoOnline.Controllers
{
    public class DoanhMucController : Controller
    {
        private DataClasses1DataContext db = new DataClasses1DataContext();

        public ActionResult Index(int? page, int? categoryId)
        {
            if (!categoryId.HasValue)
            {
                return RedirectToAction("Index", "Home"); // Redirect to a general page or display a message
            }

            var categoryName = db.DANHMUCs
                .Where(d => d.MaDM == categoryId)
                .Select(d => d.TenDM)
                .FirstOrDefault();

            if (categoryName == null)
            {
                return RedirectToAction("Index", "Home"); // Redirect to a general page or display a message
            }

            const int productsPerPage = 10;
            var pageNumber = page ?? 1;

            var sanPhams = db.SANPHAMs
                .Where(s => s.MaDM == categoryId)
                .OrderByDescending(s => s.NgayCapNhat)
                .ToPagedList(pageNumber, productsPerPage);

            var viewModel = new CategoryProductsViewModel
            {
                CategoryName = categoryName,
                Products = sanPhams
            };

            return View(viewModel);
        }
    }
}



public class CategoryProductsViewModel
{
    public string CategoryName { get; set; }
    public IPagedList<SANPHAM> Products { get; set; }
}

