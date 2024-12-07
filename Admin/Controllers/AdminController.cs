using PagedList;
using QuanLyPhongTro;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Macs;
using Admin.Filters;
using Admin.Helpers;

namespace Admin.Controllers
{
    [RequireLogin]
    public class AdminController : Controller
    {
        private QLPhongTro ql = new QLPhongTro();
        public ActionResult Login(string success)
        {
            //Câu lệnh truyền vào view khi CRUD thành công
            if (!string.IsNullOrEmpty(success))
            {
                TempData["success"] = success;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(string sdt, string password)
        {
            //Kiểm tra tài khoản có trong database không
            var user = ql.TaiKhoan.FirstOrDefault(u => u.SoDienThoai == sdt && u.MatKhau == password);

            if (user != null)
            {
                //Kiểm tra chức vụ
                if (user.ChucVu.Contains("Quản lý"))
                {
                    Session["TaiKhoan"] = user.SoDienThoai;
                    return RedirectToAction("Phong");
                }
                else
                {
                    Session["TaiKhoan"] = user.SoDienThoai;
                    return RedirectToAction("Index","Home");
                }
            }
            //Thông báo không có tài khoản trên
            TempData["err"] = "Tài khoản hoặc mật khẩu không đúng";
            return View();
        }
        public ActionResult Phong(int? page)
        {
            // Truyền thông báo CRUD từ TempData
            if (TempData["success"] != null)
            {
                ViewData["success"] = TempData["success"];
            }
            if (TempData["err"] != null)
            {
                ViewData["err"] = TempData["err"];
            }
            // Chỉ số cho 1 size trang
            int pageSize = 6;
            // Số trang
            int pageNumber = (page ?? 1);
            // Lấy danh sách phòng theo mã phòng tăng dần
            var phong = ql.Phong.OrderBy(p => p.MaPhong).ToPagedList(pageNumber, pageSize);

            return View(phong);
        }

        //Tìm kiếm phòng
        public ActionResult Search(string searchString, int? page, bool? daThue)
        {
            //Truy vấn tất acr phòng
            var phongs = from p in ql.Phong select p;
            //Kiểm tra xem chuỗi nhập vào có null không
            if (!String.IsNullOrEmpty(searchString) && searchString!="")
            {
                phongs = phongs.Where(p => p.SoPhong.Contains(searchString));
            }
            //Kiểm tra xem trạng thái combobox người dùng chọn
            if (daThue.HasValue)
            {
                phongs = phongs.Where(p => p.DaThue == daThue.Value);
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View("Phong", phongs.OrderBy(p => p.MaPhong).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SearchDien(string searchString, int? page)
        {
            var dien = from p in ql.Dien select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                //Truy vấn theo chuỗi người dùng nhập vào và lấy ra danh sách
                var maList = ql.Phong
                    .Where(p => p.SoPhong.Contains(searchString))
                    .Select(p => p.MaPhong)
                    .ToList();
                
                dien = dien.Where(p => p.MaPhong.HasValue && maList.Contains(p.MaPhong.Value));
            }


            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View("Dien", dien.OrderBy(p => p.MaPhong).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SearchLoaiPhong(string searchString, int? page)
        {
            var dien = from p in ql.Dien select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                //Truy vấn theo chuỗi người dùng nhập vào và lấy ra danh sách
                var maList = ql.Phong
                    .Where(p => p.SoPhong.Contains(searchString))
                    .Select(p => p.MaPhong)
                    .ToList();

                dien = dien.Where(p => p.MaPhong.HasValue && maList.Contains(p.MaPhong.Value));
            }


            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View("Dien", dien.OrderBy(p => p.MaPhong).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SearchNuoc(string searchString, int? page)
        {
            var nuoc = from p in ql.Nuoc select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                var maList = ql.Phong
                    .Where(p => p.SoPhong.Contains(searchString))
                    .Select(p => p.MaPhong)
                    .ToList();

                nuoc = nuoc.Where(p => p.MaPhong.HasValue && maList.Contains(p.MaPhong.Value));
            }


            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View("Nuoc", nuoc.OrderBy(p => p.MaPhong).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SearchHD(string searchString, int? page)
        {
            var hd = from p in ql.HoaDon select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                hd = hd.Where(p => p.Phong.SoPhong.Contains(searchString));
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View("DH", hd.OrderBy(p => p.MaHoaDon).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SearchTen(string searchString, int? page)
        {
            var kh = from p in ql.KhachThue select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                kh = kh.Where(p => p.Ten.Contains(searchString));
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View("KH", kh.OrderBy(p => p.MaKhachThue).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SearchTK(string searchString, int? page,string chucvu)
        {
            var tk = from p in ql.TaiKhoan select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                tk = tk.Where(p => p.SoDienThoai.Contains(searchString));
            }

            if (!String.IsNullOrEmpty(chucvu))
            {
                tk = tk.Where(p=> p.ChucVu.Contains(chucvu));
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View("TaiKhoan", tk.OrderBy(p => p.MaTaiKhoan).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ThongKe(int page = 1, int pageSize = 6)
        {
            page = page < 1 ? 1 : page;

            ql.Database.ExecuteSqlCommand("EXEC UpdateThongKe");

            var thongKeData = ql.ThongKe
                                .OrderBy(t => t.Thang)
                                .ToPagedList(page, pageSize);
            ViewBag.SelectedSize = pageSize;

            return View(thongKeData);
        }
        [HttpGet] 
        public ActionResult AddRoom()
        {
            // Lấy danh sách loại phòng từ database
            var roomTypes = ql.LoaiPhong.Select(lp => new SelectListItem
            {
                Text = lp.TenLP,
                Value = lp.MaLP.ToString()
            }).ToList();

            // Thêm danh sách loại phòng vào thẻ LoaiPhongOptions
            ViewBag.LoaiPhongOptions = roomTypes;

            return View();
        }
        [HttpPost]
        public ActionResult AddRoom(Phong phong)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem phòng có tồn tại không
                bool roomExists = ql.Phong.Any(p => p.SoPhong == phong.SoPhong);
                if (roomExists)
                {
                    TempData["err"] = "Số phòng đã tồn tại.";
                    return View(phong);
                }
                // Thêm phòng
                ql.Phong.Add(phong);
                ql.SaveChanges();
                // Sử dụng TempData để lưu thông báo
                TempData["success"] = "Bạn đã thêm phòng thành công";
                return RedirectToAction("Phong");
            }
            // Tạo lại danh sách LoaiPhongOptions mới
            ViewBag.LoaiPhongOptions = ql.LoaiPhong.Select(lp => new SelectListItem
            {
                Text = lp.TenLP,
                Value = lp.MaLP.ToString()
            }).ToList();

            return View(phong);
        }
        [HttpPost]
        public JsonResult AddRoomType(string roomTypeName)
        {
            if (string.IsNullOrWhiteSpace(roomTypeName))
            {
                return Json(new { success = false, message = "Tên loại phòng không được để trống." });
            }

            try
            {
                // Add the new room type to the database
                var newRoomType = new LoaiPhong
                {
                    TenLP = roomTypeName
                };
                ql.LoaiPhong.Add(newRoomType);
                ql.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi: " + ex.Message });
            }
        }
        public ActionResult EditRoom(int id)
        {
            //Truy vấn phòng theo mã phòng
            var room = ql.Phong.SingleOrDefault(phong => phong.MaPhong == id);
            if (room == null)
            {
                return HttpNotFound();
            }

            // Truy vấn danh sách loại phòng từ database
            var roomTypes = ql.LoaiPhong
                             .Select(lp => new SelectListItem
                             {
                                 Text = lp.TenLP,   // Room type name
                                 Value = lp.MaLP.ToString() // Room type ID or unique value
                             }).ToList();

            // Lưu danh sách loại phòng vào ViewBag
            ViewBag.LoaiPhongOptions = roomTypes;
            return View(room);
        }
        [HttpPost]
        public ActionResult EditRoom(Phong phong)
        {
            if (ModelState.IsValid)
            {
                Phong existingRoom = ql.Phong.Find(phong.MaPhong);

                if (existingRoom == null)
                {
                    return HttpNotFound("Không tìm thấy phòng để cập nhật");
                }
                //Cố set giá trị cho existingRoom
                ql.Entry(existingRoom).CurrentValues.SetValues(phong);

                ql.SaveChanges();
                TempData["success"] = "Bạn đã sửa phòng thành công";
                return RedirectToAction("Phong");
            }
            return View(phong);
        }

        public ActionResult DeleteRoom(int id)
        {
            Phong roomToDelete = ql.Phong.Find(id);

            if (roomToDelete == null)
            {
                return HttpNotFound("Không tìm thấy phòng để xóa");
            }


            var khachThue = ql.KhachThue.FirstOrDefault(k => k.MaPhong == id);
            if (khachThue != null)
            {
                TempData["error"] = "Không thể xóa phòng đang được cho thuê.";
                return RedirectToAction("Phong");
            }
            //Xóa chỉ số điện trước
            var dienRecords = ql.Dien.Where(d => d.MaPhong == id);
            ql.Dien.RemoveRange(dienRecords);
            //Xóa chỉ số nước
            var nuocRecords = ql.Nuoc.Where(n => n.MaPhong == id);
            ql.Nuoc.RemoveRange(nuocRecords);
            //Xóa chi tiết hóa đơn
            var chiTietHoaDonList = ql.ChiTietHoaDon.Where(ct => ct.MaPhong == id);
            ql.ChiTietHoaDon.RemoveRange(chiTietHoaDonList);
            var HoaDonList = ql.HoaDon.Where(ct => ct.MaPhong == id);
            ql.HoaDon.RemoveRange(HoaDonList);
            ql.Phong.Remove(roomToDelete);
            ql.SaveChanges();
            TempData["success"] = "Bạn đã xóa phòng thành công";
            return RedirectToAction("Phong");
        }

        public ActionResult DetailsRoom(int id)
        {
            Phong phong = ql.Phong.Find(id);

            if (phong == null)
            {
                return HttpNotFound("Không tìm thấy phòng");
            }

   
            Nuoc nuoc = ql.Nuoc.SingleOrDefault(n => n.MaPhong == id);
            Dien dien = ql.Dien.SingleOrDefault(d => d.MaPhong == id);
            List<KhachThue> khachThue = ql.KhachThue.Where(k => k.MaPhong == id).ToList();

            ViewBag.Nuoc = nuoc;
            ViewBag.Dien = dien;
            ViewBag.KhachThue = khachThue;

            return View(phong);
        }
        public ActionResult TinhTienPhong(int maPhong)
        {
            Phong phong = ql.Phong.Find(maPhong);

            if (phong == null)
            {
                return HttpNotFound("Không tìm thấy phòng.");
            }
            //Truy vấn điên và nước của phòng
            Dien dien = ql.Dien.FirstOrDefault(d => d.MaPhong == maPhong);
            Nuoc nuoc = ql.Nuoc.FirstOrDefault(n => n.MaPhong == maPhong);

            if (dien == null || nuoc == null)
            {
                return RedirectToAction("Phong", new { error = "Phòng này chưa tính chỉ số nước hoặc chỉ số điện." });
            }
            var kh = ql.KhachThue.FirstOrDefault(k => k.MaPhong == maPhong);
            if (kh == null)
            {
                return HttpNotFound("Không tìm thấy khách thuê.");
            }
            //Hàm tính tiền đơn giản
            decimal tienPhong = Decimal.Round(phong.GiaThueThang);
            decimal tienDien = Decimal.Round((dien.ChiSoMoi - dien.ChiSoCu) * dien.GiaTien);
            decimal tienNuoc = Decimal.Round((nuoc.ChiSoMoi - nuoc.ChiSoCu) * nuoc.GiaTien);
            decimal thanhTien = Decimal.Round(tienPhong + tienDien + tienNuoc);
            System.Diagnostics.Debug.WriteLine($"tienPhong: {tienPhong}, tienDien: {tienDien}, tienNuoc: {tienNuoc}, thanhTien: {thanhTien}");

            //Tạo lập 1 hóa đơn 
            HoaDon hoadon = new HoaDon
            {
                NgayLap = DateTime.Now, 
                ThanhTien = thanhTien, 
                MaPhong = maPhong ,
                MaKH = kh.MaKhachThue
            };
            //Thêm hóa đơn
            ql.HoaDon.Add(hoadon);
            ql.SaveChanges();
            //Lấy hóa đơn vừa tạo
            int maHoaDon = ql.HoaDon.Max(u=> u.MaHoaDon);
            //tạo chi tiết hóa đơn từ hóa đơn vừa tạo
            ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon
            {
                MaHoaDon = maHoaDon, 
                MaPhong = maPhong, 
                TienPhong = tienPhong, 
                TienDien = tienDien,
                TienNuoc = tienNuoc
            };

            ql.ChiTietHoaDon.Add(chiTietHoaDon);
            ql.SaveChanges();
            // Compose and send the email
            string recipientEmail = kh.Mail; // Ensure KhachThue table includes Email column
            string subject = $"Hóa đơn tiền phòng cho phòng {phong.SoPhong}";
            string body = $@"
                <h1>Hóa đơn tiền phòng</h1>
                <p><strong>Phòng:</strong> {phong.SoPhong}</p>
                <p><strong>Tiền phòng:</strong> {tienPhong:C}</p>
                <p><strong>Tiền điện:</strong> {tienDien:C}</p>
                <p><strong>Tiền nước:</strong> {tienNuoc:C}</p>
                <p><strong>Tổng cộng:</strong> {thanhTien:C}</p>
                <p>Ngày lập hóa đơn: {DateTime.Now:dd/MM/yyyy}</p>";

            try
            {
                EmailHelper emailHelper = new EmailHelper();
                emailHelper.SendEmail(recipientEmail, subject, body);
            }
            catch (Exception ex)
            {
                // Handle email sending errors
                return RedirectToAction("Phong", new { error = "Không thể gửi email: " + ex.Message });
            }

            return RedirectToAction("HoaDon", new { maHoaDon = maHoaDon });
        }
        public ActionResult ChiTietHD(int id)
        {
            var cthd = ql.ChiTietHoaDon.SingleOrDefault(u=>u.MaHoaDon ==id);
            return View(cthd);
        }
        [HttpPost]
        public ActionResult SendNotice()
        {
            try
            {
                var emailHelper = new EmailHelper();
                string recipientEmail = "recipient@example.com"; // Replace with the recipient's email
                string subject = "Important Notice";
                string body = "<p>This is an important notice sent from the Admin system.</p>";

                emailHelper.SendEmail(recipientEmail, subject, body);

                ViewBag.Message = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error sending email: " + ex.Message;
            }

            return View();
        }
        public ActionResult InHoaDonPDF(int maHoaDon)
        {
            HoaDon hoadon = ql.HoaDon.Find(maHoaDon);

            if (hoadon == null)
            {
                return HttpNotFound("Không tìm thấy hóa đơn.");
            }
            Document doc = new Document();
            MemoryStream memoryStream = new MemoryStream();

            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
            doc.Open();

            Paragraph content = new Paragraph();
            content.Add(new Phrase("Noi dung hoa don:\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)));

            content.Add(new Chunk($"Ngay lap: {hoadon.NgayLap}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
            content.Add(new Chunk($"Tong tien: {hoadon.ThanhTien}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));

            content.Add(new Phrase("Chi tiet hoa don:\n", FontFactory.GetFont(FontFactory.HELVETICA, 12)));

            foreach (var chiTiet in hoadon.ChiTietHoaDon)
            {
                content.Add(new Chunk($"Ma phong: {chiTiet.MaPhong}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                content.Add(new Chunk($"Tien phong: {chiTiet.TienPhong}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                content.Add(new Chunk($"Tien dien: {chiTiet.TienDien}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
                content.Add(new Chunk($"Tien nuoc: {chiTiet.TienNuoc}\n", FontFactory.GetFont(FontFactory.HELVETICA, 10)));
            }

            doc.Add(content);

            doc.Close();

            byte[] pdfBytes = memoryStream.ToArray();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Hoadon.pdf");
            Response.OutputStream.Write(pdfBytes, 0, pdfBytes.Length);

            return new EmptyResult();
        }
        public ActionResult Hoadon(int maHoaDon)
        {
            HoaDon hoadon = ql.HoaDon.Find(maHoaDon);

            if (hoadon == null)
            {
                return HttpNotFound("Không tìm thấy hóa đơn.");
            }

            hoadon.ChiTietHoaDon = ql.ChiTietHoaDon.Where(ct => ct.MaHoaDon == maHoaDon).ToList();

            return View(hoadon);
        }

        public ActionResult Dien(int? page,string success)
        {
            if (!string.IsNullOrEmpty(success))
            {
                TempData["success"] = success;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var dien = ql.Dien.OrderBy(p => p.MaDien).ToPagedList(pageNumber, pageSize);
            return View(dien);
        }

        [HttpGet]
        public ActionResult AddDien()
        {
            ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
            return View();
        }

        [HttpPost]
        public ActionResult AddDien(Dien dien)
        {
            if (ModelState.IsValid)
            {
                if (ql.Dien.Any(d => d.MaPhong == dien.MaPhong))
                {
                    TempData["err"] = "Phòng này đã có chỉ số điện.";
                    ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
                    return View(dien);
                }

                dien.GiaTien = (dien.ChiSoMoi - dien.ChiSoCu) * dien.GiaTien;
                ql.Dien.Add(dien);
                ql.SaveChanges();
                TempData["success"] = "Bạn đã thêm chỉ số điện thành công";
                return RedirectToAction("Dien");
            }
            ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
            return View(dien);
        }

        public ActionResult EditDien(int id)
        {
            ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
            var room = ql.Dien.SingleOrDefault(u => u.MaDien == id);
            room.GiaTien = room.GiaTien / (room.ChiSoMoi - room.ChiSoCu);
            return View(room);
        }
        [HttpPost]
        public ActionResult EditDien(Dien dien)
        {
            if (ModelState.IsValid)
            {
                Dien existingRoom = ql.Dien.Find(dien.MaDien);

                if (existingRoom == null)
                {
                    return HttpNotFound("Không tìm thấy điện để cập nhật");
                }

                if (ql.Dien.Any(d => d.MaPhong == dien.MaPhong && d.MaDien != dien.MaDien))
                {
                    TempData["err"] = "Phòng này đã có chỉ số điện vui lòng chọn lại .";
                    ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
                    return View(dien);
                }
                dien.GiaTien = (dien.ChiSoMoi - dien.ChiSoCu) * dien.GiaTien;
                ql.Entry(existingRoom).CurrentValues.SetValues(dien);
                ql.SaveChanges();
                TempData["success"] = "Bạn đã sửa chỉ số điện thành công";
                return RedirectToAction("Dien");
            }
            ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
            return View(dien);
        }


        public ActionResult DeleteDien(int id)
        {
            Dien roomToDelete = ql.Dien.Find(id);

            if (roomToDelete == null)
            {
                return HttpNotFound("Không tìm thấy điện để xóa");
            }

            ql.Dien.Remove(roomToDelete);
            ql.SaveChanges();
            TempData["success"] = "Bạn đã xóa chỉ số điện thành công";
            return RedirectToAction("Dien");
        }
        public ActionResult Nuoc(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var nuoc = ql.Nuoc.Include("Phong").OrderBy(p => p.MaNuoc).ToPagedList(pageNumber, pageSize);
            return View(nuoc);
        }

        [HttpGet]
        public ActionResult AddNuoc()
        {
            ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
            return View();
        }

        [HttpPost]
        public ActionResult AddNuoc(Nuoc nuoc)
        {
            if (ModelState.IsValid)
            {
                if (ql.Nuoc.Any(d => d.MaPhong == nuoc.MaPhong))
                {
                    TempData["err"] = "Phòng này đã có chỉ số nước.";
                    ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
                    return View(nuoc);
                }
                if (nuoc.ChiSoMoi == 0 || nuoc.ChiSoCu == 0)
                {
                    TempData["err"] = "Vui lòng nhập đầy đủ chỉ số nước cũ và mới";
                    ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
                    return View(nuoc);
                }
                else if (nuoc.ChiSoMoi == nuoc.ChiSoCu)
                {
                    nuoc.GiaTien = 0;
                }
                else if (nuoc.ChiSoMoi > nuoc.ChiSoCu)
                {
                    nuoc.GiaTien = (nuoc.ChiSoMoi - nuoc.ChiSoCu) * nuoc.GiaTien;
                }
                else
                {
                    TempData["err"] = "Chỉ số mới phải lớn hơn chỉ số cũ.";
                    ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
                    return View(nuoc);
                }
                ql.Nuoc.Add(nuoc);
                ql.SaveChanges();
                TempData["success"] = "Bạn đã thêm chỉ số nước thành công";
                return RedirectToAction("Nuoc");
            }

            // Nếu ModelState không hợp lệ, trả về view kèm dữ liệu cần thiết
            ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
            return View(nuoc);
        }

        public ActionResult EditNuoc(int id)
        {
            ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
            var room = ql.Nuoc.SingleOrDefault(u => u.MaNuoc == id);
            room.GiaTien = room.GiaTien / (room.ChiSoMoi - room.ChiSoCu);
            return View(room);
        }
        [HttpPost]
        public ActionResult EditNuoc(Nuoc nuoc)
        {
            if (ModelState.IsValid)
            {
                Nuoc existingRoom = ql.Nuoc.Find(nuoc.MaNuoc);

                if (existingRoom == null)
                {
                    return HttpNotFound("Không tìm thấy nước để cập nhật");
                }

                if (ql.Nuoc.Any(d => d.MaPhong == nuoc.MaPhong && d.MaNuoc != nuoc.MaNuoc))
                {
                    TempData["err"] = "Phòng này đã có chỉ số nước vui lòng chọn lại.";
                    ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
                    return View(nuoc);
                }

                // Kiểm tra và tính lại giá tiền
                if (nuoc.ChiSoMoi > nuoc.ChiSoCu)
                {
                    nuoc.GiaTien = (nuoc.ChiSoMoi - nuoc.ChiSoCu) * nuoc.GiaTien;
                    TempData["success"] = "Bạn đã cập nhật thành công";
                }
                else
                {
                    TempData["err"] = "Chỉ số mới phải lớn hơn chỉ số cũ.";
                    ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
                    return View(nuoc);
                }

                ql.Entry(existingRoom).CurrentValues.SetValues(nuoc);
                ql.SaveChanges();
 
                return RedirectToAction("Nuoc");
            }
            ViewBag.MaPhongList = new SelectList(ql.Phong, "MaPhong", "SoPhong");
            return View(nuoc);
        }

        public ActionResult DeleteNuoc(int id)
        {
            Nuoc roomToDelete = ql.Nuoc.Find(id);

            if (roomToDelete == null)
            {
                return HttpNotFound("Không tìm thấy nước để xóa");
            }

            ql.Nuoc.Remove(roomToDelete);
            ql.SaveChanges();
            TempData["success"] = "Bạn đã xóa chỉ số nước thành công";

            return RedirectToAction("Nuoc");
        }
        public ActionResult KH(int? page, string success)
        {
            if (!string.IsNullOrEmpty(success))
            {
                ViewData["success"] = success;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var khachThues = ql.KhachThue.OrderBy(p => p.MaPhong).ToPagedList(pageNumber, pageSize);
            return View(khachThues);
        }

        [HttpGet]
        public ActionResult AddKH()
        {
            ViewBag.MaPhongList = new SelectList(ql.Phong.Where(u=>u.DaThue==false), "MaPhong", "SoPhong");
            return View();
        }
        public bool IsPhoneNumberValid(string phoneNumber)
        {
            // Số điện thoại phải có đúng 10 số
            var phoneNumberRegex = new Regex(@"^\d{10}$");
            return phoneNumberRegex.IsMatch(phoneNumber);
        }

        public bool IsVietnameseIDValid(string vietnameseID)
        {
            // Số CMND phải có 9 hoặc 12 chữ số (đúng định dạng CMND Việt Nam)
            var vietnameseIDRegex = new Regex(@"^\d{9}$|^\d{12}$");
            return vietnameseIDRegex.IsMatch(vietnameseID);
        }

        [HttpPost]
        public ActionResult AddKH(KhachThue khach)
        {
            bool sdtExists = ql.KhachThue.Any(tk => tk.DienThoai == khach.DienThoai);

            if (sdtExists)
            {
                ViewBag.MaPhongList = new SelectList(ql.Phong.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                TempData["err"] = "Số điện thoại đã tồn tại.";
                return View(khach);
            }
            bool cmndExists = ql.KhachThue.Any(tk => tk.CMND == khach.CMND);
            if (cmndExists)
            {
                ViewBag.MaPhongList = new SelectList(ql.Phong.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                TempData["err"] = "Số CMND đã tồn tại.";
                return View(khach);
            }
            if (khach.HoTenDem == null || khach.Ten == null || khach.CMND == null || khach.DienThoai == null || khach.NgayVaoO == null || khach.MaPhong==null)
            {
                ViewBag.MaPhongList = new SelectList(ql.Phong.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                TempData["err"] = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }
            if (!IsPhoneNumberValid(khach.DienThoai))
            {
                ViewBag.MaPhongList = new SelectList(ql.Phong.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                TempData["err"] = "Số điện thoại không hợp lệ. Số điện thoại phải có 10 số.";
                return View();
            }

            if (!IsVietnameseIDValid(khach.CMND))
            {
                ViewBag.MaPhongList = new SelectList(ql.Phong.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
                TempData["err"] = "Số CCCD không hợp lệ. Số CCCD phải có 12 chữ số.";
                return View();
            }
            if (ModelState.IsValid)
            {
                ql.KhachThue.Add(khach);
                ql.SaveChanges();
                Phong phong = ql.Phong.SingleOrDefault(p => p.MaPhong == khach.MaPhong);
                if (phong != null)
                {
                    phong.DaThue = true;
                    ql.SaveChanges();
                    TempData["success"] = "Bạn đã thêm khách thuê trọ thành công";
                }
                return RedirectToAction("KH");
            }
            ViewBag.MaPhongList = new SelectList(ql.Phong.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
            return View();
        }
        public ActionResult EditKH(int id)
        {
            ViewBag.MaPhongList = new SelectList(ql.Phong.Where(u => u.DaThue == false), "MaPhong", "SoPhong");
            var khach = ql.KhachThue.SingleOrDefault(u => u.MaKhachThue == id);
            return View(khach);
        }
        [HttpPost]
        public ActionResult EditKH(KhachThue khach, int MaPhongCu)
        {
            if (ModelState.IsValid)
            {
                KhachThue existingRoom = ql.KhachThue.Find(khach.MaKhachThue);

                if (existingRoom == null)
                {
                    return HttpNotFound("Không tìm thấy khách hàng để cập nhật");
                }

                ql.Entry(existingRoom).CurrentValues.SetValues(khach);
                Phong phongCu = ql.Phong.SingleOrDefault(p => p.MaPhong == MaPhongCu);
                Phong phongMoi = ql.Phong.SingleOrDefault(p => p.MaPhong == khach.MaPhong);

                if (phongCu != null && phongMoi != null)
                {
                    phongCu.DaThue = false;
                    phongMoi.DaThue = true;
                    ql.SaveChanges();
                }
                else
                {
                    existingRoom.MaPhong = MaPhongCu;
                    ql.SaveChanges();
                }
                TempData["success"] = "Bạn đã sửa thông tin khách hàng thành công";
                return RedirectToAction("KH");
            }
            return View(khach);
        }

        public ActionResult DeleteKH(int id)
        {
            KhachThue roomToDelete = ql.KhachThue.Find(id);

            if (roomToDelete == null)
            {
                return HttpNotFound("Không tìm thấy khách hàng để xóa");
            }
            Phong phong = ql.Phong.SingleOrDefault(p => p.MaPhong == roomToDelete.MaPhong);
            if (phong != null)
            {
                phong.DaThue = false;
                ql.SaveChanges();
            }
            var dh = ql.HoaDon.Where(u=>u.MaKH==id);
            foreach (var cthd in dh)
            {
                var ct = ql.ChiTietHoaDon.SingleOrDefault(u => u.MaHoaDon == cthd.MaHoaDon);
                ql.ChiTietHoaDon.Remove(ct);
                ql.HoaDon.Remove(cthd);
            }
            ql.KhachThue.Remove(roomToDelete);
            ql.SaveChanges();
            TempData["success"] = "Bạn đã xóa khách hàng thành công";
            return RedirectToAction("KH");
        }
        public ActionResult DH(int? page)
        {
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var hd = ql.HoaDon.OrderBy(p => p.MaHoaDon).ToPagedList(pageNumber, pageSize);
            return View(hd);
        }
        public ActionResult TaiKhoan(int? page, string success)
        {
            if (!string.IsNullOrEmpty(success))
            {
                ViewData["success"] = success;
            }
            int pageSize = 6;
            int pageNumber = (page ?? 1);
            var hd = ql.TaiKhoan.OrderBy(p => p.MaTaiKhoan).ToPagedList(pageNumber, pageSize);
            return View(hd);
        }
        [HttpGet]
        public ActionResult CreateTK()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTK(TaiKhoan taiKhoan, string NhapLaiMatKhau)
        {
            if (ModelState.IsValid)
            {
                bool sdtExists = ql.TaiKhoan.Any(tk => tk.SoDienThoai == taiKhoan.SoDienThoai);

                if (sdtExists)
                {
                    ViewData["err"] = "Số điện thoại đã tồn tại.";
                    return View(taiKhoan);
                }
                sdtExists = ql.KhachThue.Any(tk => tk.DienThoai == taiKhoan.SoDienThoai); 
                if (!sdtExists)
                {
                    ViewData["err"] = "Số điện thoại bạn đang tạo không trùng với bất kì số điện thoại nào của khách hàng.";
                    return View(taiKhoan);
                }
                if (taiKhoan.MatKhau != NhapLaiMatKhau)
                {
                    ViewData["err"] = "Mật khẩu và nhập lại mật khẩu không khớp.";
                    return View(taiKhoan);
                }
                if (!IsPhoneNumberValid(taiKhoan.SoDienThoai))
                {
                    ViewData["err"] = "Số điện thoại không hợp lệ. Số điện thoại phải có 10 số.";
                    return View(taiKhoan);
                }
                ql.TaiKhoan.Add(taiKhoan);
                ql.SaveChanges();
                return RedirectToAction("TaiKhoan",new {success = "Bạn đã tạo tài khoản mới thành công" });
            }

            return View(taiKhoan);
        }
        public ActionResult EditTK(int id)
        {
            TaiKhoan taiKhoan = ql.TaiKhoan.Find(id);

            if (taiKhoan == null)
            {
                return HttpNotFound("Không tìm thấy tài khoản để chỉnh sửa.");
            }

            return View(taiKhoan);
        }
        [HttpPost]
        public ActionResult EditTK(TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                bool sdtExists = ql.TaiKhoan.Any(tk => tk.SoDienThoai == taiKhoan.SoDienThoai && tk.MaTaiKhoan != taiKhoan.MaTaiKhoan);

                if (sdtExists)
                {
                    ViewData["err"] = "Số điện thoại đã tồn tại.";
                    return View(taiKhoan);
                }

                TaiKhoan existingTaiKhoan = ql.TaiKhoan.Find(taiKhoan.MaTaiKhoan);

                if (existingTaiKhoan == null)
                {
                    return HttpNotFound("Không tìm thấy tài khoản để cập nhật.");
                }
                if (!IsPhoneNumberValid(taiKhoan.SoDienThoai))
                {
                    ViewData["err"] = "Số điện thoại không hợp lệ. Số điện thoại phải có 10 số.";
                    return View(taiKhoan);
                }
                ql.Entry(existingTaiKhoan).CurrentValues.SetValues(taiKhoan);
                ql.SaveChanges();
                return RedirectToAction("TaiKhoan",new {success= "Bạn đã sửa tài khoản thành công" });
            }

            return View(taiKhoan);
        }
        //Action cấp lại mật khẩu cho người dùng
        public ActionResult Pass(int id,string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewData["err"] = error;
            }
            ViewBag.UserId = id;
            return View();
        }

        [HttpPost]
        public ActionResult Pass(int id, string newPassword, string confirmPassword)
        {
            if (newPassword!=confirmPassword)
            {

                return RedirectToAction("Pass",new {id=id,error = "Mật khẩu phải trùng" });
            }
                // Lấy tài khoản từ cơ sở dữ liệu theo 'id' (sử dụng Entity Framework)
                var taiKhoan = ql.TaiKhoan.SingleOrDefault(tk => tk.MaTaiKhoan == id);

                if (taiKhoan != null)
                {
                    // Cập nhật mật khẩu của tài khoản
                    taiKhoan.MatKhau = newPassword;
                taiKhoan.TrangThai = true;
                    // Lưu thay đổi vào cơ sở dữ liệu
                    ql.SaveChanges();

                    return RedirectToAction("TaiKhoan", new { success = "Bạn đã cấp mật khẩu cho tài khoản thành công" });
                }
                else
                {
                return RedirectToAction("TaiKhoan", new { error = "Bạn đã cấp mật khẩu cho tài khoản không thành công" });
            }
         
        }
        //Action khi người dùng bấm vào quên mật khẩu
        [HttpGet]
        public ActionResult Forget(string error)
        {
            if (!string.IsNullOrEmpty(error))
            {
                ViewData["err"] = error;
            }
            return View();
        }
        //Hàm xử lý khi người dùng nhấn nút gửi
        [HttpPost]
        public ActionResult Forget(string phoneNumber, string s)
        {
            var tk = ql.TaiKhoan.SingleOrDefault(u=>u.SoDienThoai == phoneNumber);
            if (tk==null)
            {
                return RedirectToAction("Forget", new { error = "Số điện thoại không tồn tại trong hệ thống" });
            }
            tk.TrangThai = false;
            ql.SaveChanges();
            return RedirectToAction("Login", new { success = "Yêu cầu thành công vui lòng liên hệ admin để cấp lại mật khẩu" });
        }
        public ActionResult DeleteTK(int id)
        {
            TaiKhoan taiKhoan = ql.TaiKhoan.Find(id);

            if (taiKhoan == null)
            {
                return HttpNotFound("Không tìm thấy tài khoản để xóa.");
            }

            ql.TaiKhoan.Remove(taiKhoan);
            ql.SaveChanges();

            return RedirectToAction("TaiKhoan",new {success = "Bạn đã xóa tài khoản thành công"});
        }


    }
}