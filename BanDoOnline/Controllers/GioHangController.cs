using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BanDoOnline.Models;
using PayPal.Api;

namespace BanDoOnline.Controllers
{
    public class GioHangController : Controller
    {
        DataClasses1DataContext db = new DataClasses1DataContext(); 

        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }

        public List<GioHang> LayGioHang()
        {
            // Lấy giỏ hàng từ Session hoặc cơ chế lưu trữ khác
            List<GioHang> gioHang = Session["GioHang"] as List<GioHang>;

            // Nếu giỏ hàng chưa tồn tại, khởi tạo nó
            if (gioHang == null)
            {
                gioHang = new List<GioHang>();
                Session["GioHang"] = gioHang;
            }

            return gioHang;
        }



        public ActionResult ThemGioHang(int ms, int quantity = 1, int sizeId = 0, int colorId = 0)
        {
            // Các bước khác để thêm vào giỏ hàng

            // Thêm thông tin size và color đã chọn vào giỏ hàng
            ThemSanPhamVaoGioHang(ms, quantity, sizeId, colorId);

            // Chuyển người dùng đến trang giỏ hàng
            return RedirectToAction("Index", "GioHang");
        }


        public ActionResult ThemGioHang1(int ms, string url)
        {
            ThemSanPhamVaoGioHang(ms);
            return Redirect(url);
        }

        // Hàm chung để thêm sản phẩm vào giỏ hàng
        // Hàm để thêm sản phẩm vào giỏ hàng
        private void ThemSanPhamVaoGioHang(int ms, int quantity = 1, int sizeId = 0, int colorId = 0)
        {
            // Lấy giỏ hàng hiện tại
            List<GioHang> lstGioHang = LayGioHang();

            // Kiểm tra nếu sản phẩm chưa có trong giỏ hàng thì thêm vào, nếu đã có thì tăng số lượng
            GioHang sp = lstGioHang.Find(n => n.MaSanPham == ms && n.MaSize == sizeId && n.MaColor == colorId);

            if (sp == null)
            {
                sp = new GioHang(ms, sizeId, colorId)
                {
                    SoLuong = quantity,
                    // Gán giá trị cho các thuộc tính mới thêm vào
                    MaSizeChon = sizeId,
                    MaColorChon = colorId,
                    TenSizeChon = db.SIZEs.SingleOrDefault(s => s.MaSize == sizeId)?.TenSize,
                    TenColorChon = db.COLORs.SingleOrDefault(c => c.MaColor == colorId)?.TenColor
                };

                lstGioHang.Add(sp);
            }
            else
            {
                sp.SoLuong += quantity;
            }

            // Lưu giỏ hàng vào Session
            Session["GioHang"] = lstGioHang;
        }


        // Hàm để lấy tổng số lượng sản phẩm trong giỏ hàng
        private int LayTongSoLuongGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            return lstGioHang.Sum(item => item.SoLuong);
        }



        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)

            {
                iTongSoLuong = lstGioHang.Sum(n => n.SoLuong);

            }

            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return dTongTien;

        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");

            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);

        }
        public ActionResult GiohangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaSPKhoiGioHang(int MaSanPham)
        {
            List<GioHang> lstGioHang = LayGioHang();

            // Kiem tra Sach da co trong Session["GioHang"]
            GioHang sp = lstGioHang.FirstOrDefault(n => n.MaSanPham == MaSanPham);

            // Xóa sp khỏi giỏ hàng
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.MaSanPham == MaSanPham);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int MaSanPham, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();

            GioHang sp = lstGioHang.FirstOrDefault(n => n.MaSanPham == MaSanPham);

            // Nếu tồn tại thì cho sửa số lượng
            if (sp != null)
            {
                sp.SoLuong = int.Parse(f["txtSoLuong"].ToString());
                sp.ThanhTien = sp.SoLuong * sp.GiaBan; // Cập nhật thành tiền
            }

            // Cập nhật tổng số lượng và tổng tiền
            Session["GioHang"] = lstGioHang;
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return RedirectToAction("GioHang");
        }




        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("SachOnline", "Home");
        }


        public ActionResult DatHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            foreach (var item in lstGioHang)
            {
                // Kiểm tra xem có tồn kho cho sản phẩm không
                var tonKho = db.CHITIETTONKHOs.SingleOrDefault(x => x.MaSP == item.MaSanPham && x.MaSize == item.MaSize && x.MaColor == item.MaColor);
                if (tonKho == null || tonKho.SoLuongTrongKho < item.SoLuong)
                {
                    // Không có tồn kho, không cần gán danh sách Sizes và Colors
                    item.Sizes = null;
                    item.Colors = null;
                }
                else
                {
                    // Lấy thông tin Size và Color từ cơ sở dữ liệu và gán vào item.Sizes và item.Colors
                    item.Sizes = db.SIZEs.ToList();
                    item.Colors = db.COLORs.ToList();
                }
            }

            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Home", "Index");
            }
           
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            try
            {
                // Get the order details from the shopping cart
                List<GioHang> lstGioHang = LayGioHang();

                // Check if the delivery date is provided
                if (string.IsNullOrEmpty(f["NgayGiao"]))
                {
                    ModelState.AddModelError("NgayGiao", "Vui lòng nhập ngày giao.");
                    ViewBag.TongSoLuong = TongSoLuong();
                    ViewBag.TongTien = TongTien();
                    return View(lstGioHang);
                }

                // Create an order and save it to the database
                DONDATHANG ddh = new DONDATHANG();
                KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
                ddh.MaKH = kh.MaKH;
                ddh.NgayDat = DateTime.Now;

                // Convert the delivery date from string to DateTime
                if (DateTime.TryParseExact(f["NgayGiao"], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ngayGiao))
                {
                    if (ngayGiao <= ddh.NgayDat)
                    {
                        ModelState.AddModelError("NgayGiao", "Ngày giao phải sau ngày đặt.");
                        ViewBag.TongSoLuong = TongSoLuong();
                        ViewBag.TongTien = TongTien();
                        return View(lstGioHang);
                    }

                    TimeSpan? timeSpan = ngayGiao - ddh.NgayDat;

                    if (timeSpan != null && timeSpan.Value.TotalDays < 3)
                    {
                        ModelState.AddModelError("NgayGiao", "Ngày giao phải cách ít nhất 3 ngày so với ngày đặt.");
                        ViewBag.TongSoLuong = TongSoLuong();
                        ViewBag.TongTien = TongTien();
                        return View(lstGioHang);
                    }

                    ddh.NgayGiao = ngayGiao;
                }
                else
                {
                    ModelState.AddModelError("NgayGiao", "Ngày giao không hợp lệ. Vui lòng nhập theo định dạng yyyy-MM-dd.");
                    ViewBag.TongSoLuong = TongSoLuong();
                    ViewBag.TongTien = TongTien();
                    return View(lstGioHang);
                }

                ddh.TinhTrangGiaoHang = 1;
                ddh.DaThanhToan = false;
                db.DONDATHANGs.InsertOnSubmit(ddh);
                db.SubmitChanges(); // Save changes to get the generated MaDonHang

                // Insert details record
                foreach (var item in lstGioHang)
                {
                    CHITIETDATHANG chiTietDonHang = new CHITIETDATHANG();
                    chiTietDonHang.MaDonHang = ddh.MaDonHang; // Use the generated MaDonHang
                    chiTietDonHang.MaSP = item.MaSanPham;
                    chiTietDonHang.SoLuong = item.SoLuong;
                    chiTietDonHang.DonGia = item.DonGia; // Set the actual price here

                    // Set size and color information
                    chiTietDonHang.MaSize = item.MaSizeChon;    // Assuming MaSizeChon contains the selected size
                    chiTietDonHang.MaColor = item.MaColorChon;  // Assuming MaColorChon contains the selected color

                    db.CHITIETDATHANGs.InsertOnSubmit(chiTietDonHang);
                }

                db.SubmitChanges();

                // Send order confirmation email
                string emailNguoiNhan = kh.Email; // Get the customer's email address from the KHACHHANG object
                string hinhThucThanhToan = f["HinhThucThanhToan"];
                string httt = hinhThucThanhToan == "ThanhToanKhiNhanHang" ? "Khi nhận hàng" : "Online";

                GuiEmailDonHang(ddh, emailNguoiNhan, lstGioHang, httt);

                // Clear the shopping cart after the order is placed
                Session["GioHang"] = null;
                Session["NgayGiao"] = f["NgayGiao"];
                CapNhatSoLuongGioHang(0);

                // Redirect to the order confirmation page
                return RedirectToAction("XacNhanDonHang");
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or redirect to an error page
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi xử lý đơn hàng. Vui lòng thử lại sau.";
                return View("Error");
            }
        }

        private void GuiEmailDonHang(DONDATHANG donHang, string emailNguoiNhan, List<GioHang> gioHangItems, string hinhThucThanhToan)
        {
            // Tạo nội dung email dựa trên thông tin đơn hàng và sản phẩm
            string subject = "Thông tin đơn hàng #" + donHang.MaDonHang;
            string body = $"<h2>Thông tin đơn hàng #{donHang.MaDonHang}</h2>";
            body += $"<p>Mã đơn hàng: {donHang.MaDonHang}</p>";
            body += $"<p>Ngày đặt: {donHang.NgayDat}</p>";
            body += $"<p>Tình trạng giao hàng: {donHang.TinhTrangGiaoHang}</p>";
            // Thêm thông tin hình thức thanh toán
            body += $"<p>Hình thức thanh toán: {hinhThucThanhToan}</p>";
            body += "<h3>Danh sách sản phẩm:</h3>";
            body += "<table border='1'><tr><th>Tên sách</th><th>Đơn giá</th><th>Số lượng</th><th>Thành tiền</th></tr>";

            foreach (var item in gioHangItems)
            {
                body += "<tr>";
                body += $"<td>{item.TenSanPham}</td>";
                body += $"<td>{item.GiaBan}</td>";
                body += $"<td>{item.SoLuong}</td>";
                body += $"<td>{item.ThanhTien}</td>";
                body += "</tr>";
            }

            body += "</table>";
            // Tính tổng tiền
            double tongTien = gioHangItems.Sum(item => item.ThanhTien);
            body += $"<p>Tổng tiền: {tongTien}</p>";
            // Gửi email
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("2124802010165@student.tdmu.edu.vn", "kdto cqoq mzca rzbt"),
                    EnableSsl = true,
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("2124802010803@student.tdmu.edu.vn"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true, // Sử dụng HTML cho nội dung email
                };

                mailMessage.To.Add(emailNguoiNhan);

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi khi gửi email
                throw ex;
            }
        }

        private void CapNhatSoLuongGioHang(int soLuongMoi)
        {
            // Thực hiện logic cập nhật số lượng sản phẩm trong giỏ hàng ở đây.
            // Ví dụ, bạn có thể lưu số lượng mới vào ViewBag hoặc ViewData để sử dụng trong View.

            ViewBag.TongSoLuongGioHang = soLuongMoi;
        }
        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        public ActionResult FailureView()
        {
            return View();
        }
        public ActionResult SuccessView()
        {
            return View();
        }
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/GioHang/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    var guid = Convert.ToString((new Random()).Next(100000));
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
            //on successful payment, show success page to user.  
            return View("SuccessView");
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc  
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "USD",
                price = "1",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = "3", // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(), //Generate an Invoice No    
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}
