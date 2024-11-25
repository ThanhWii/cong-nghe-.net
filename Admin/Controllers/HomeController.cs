using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using QuanLyPhongTro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class HomeController : Controller
    {
        private QLPhongTro ql = new QLPhongTro();
        // GET: Home
        public ActionResult Index()
        {
            string s = Session["TaiKhoan"] as string;
            var khachThue = ql.KhachThue.SingleOrDefault(k => k.DienThoai.Contains(s));
            Phong phong = ql.Phong.Find(khachThue.MaPhong);
            Session["MaKH"] = khachThue.MaKhachThue;
            if (phong == null)
            {
                return HttpNotFound("Không tìm thấy phòng");
            }


            Nuoc nuoc = ql.Nuoc.SingleOrDefault(n => n.MaPhong == khachThue.MaPhong);
            Dien dien = ql.Dien.SingleOrDefault(d => d.MaPhong == khachThue.MaPhong);


            ViewBag.Nuoc = nuoc;
            ViewBag.Dien = dien;
            ViewBag.KhachThue = khachThue;

            return View(phong);
        }
        public ActionResult DonHang()
        {
            string taiKhoan = (string)Session["TaiKhoan"];
            var khach = ql.KhachThue.SingleOrDefault(u => u.DienThoai.Contains(taiKhoan));
            var danhSachHoaDon = ql.HoaDon.Include("ChiTietHoaDon").Where(hd => hd.MaKH == khach.MaKhachThue).ToList();


            return View(danhSachHoaDon);
        }

    }
}