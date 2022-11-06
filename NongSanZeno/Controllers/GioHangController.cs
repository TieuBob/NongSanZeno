using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using NongSanZeno.Models;

namespace NongSanZeno.Controllers
{
    public class GioHangController : Controller
    {
        dbNongSanZenoDataContext data = new dbNongSanZenoDataContext();
        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list == null)
            {
                list = new List<GioHang>();
                Session["GioHang"] = list;
            }
            return list;
        }

        public ActionResult ThemGioHang(int masp, string strUrl)
        {

            List<GioHang> gioHangs = LayGioHang();

            GioHang sp = gioHangs.Find(n => n.masp == masp);
            if (sp == null)
            {
                sp = new GioHang(masp);
                gioHangs.Add(sp);
                return Redirect(strUrl);
            }
            else
            {
                sp.soluong++;
                return Redirect(strUrl);
            }
        }

        private int TongSoLuong()
        {
            int Tongsoluong = 0;
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs != null)
            {
                Tongsoluong = gioHangs.Sum(n => n.soluong);
            }
            Session["TongSoLuong"] = Tongsoluong;
            return Tongsoluong;
        }

        private double TongTien()
        {
            double tongtien = 0;
            List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
            if (gioHangs != null)
            {
                tongtien = gioHangs.Sum(n => n.thanhtien);
            }
            return tongtien;
        }

        //private double TongHoaDon()
        //{
        //    double tonghoadon = 0;
        //    List<GioHang> gioHangs = Session["GioHang"] as List<GioHang>;
        //    if (gioHangs != null)
        //    {
        //        tonghoadon = gioHangs.Sum(n => n.tonghoadon);
        //    }
        //    return tonghoadon;
        //}

        public ActionResult Giohang()
        {
            List<GioHang> gioHangs = LayGioHang();
            if (gioHangs.Count == 0)
            {
                //return RedirectToAction("SanPham", "NongSanZeno");
                return RedirectToAction("GioHangNull", "GioHang");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            //ViewBag.TongHoaDon = TongHoaDon();
            return View(gioHangs);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            //ViewBag.TongHoaDon = TongHoaDon();
            return PartialView();
        }

        public ActionResult GiohangPartial1()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            //ViewBag.TongHoaDon = TongHoaDon();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> gioHangs = LayGioHang();
            GioHang sessiongiohang = gioHangs.SingleOrDefault(n => n.masp == id);
            if (sessiongiohang != null)
            {
                gioHangs.RemoveAll(n => n.masp == id);
                return RedirectToAction("GioHang");
            }
            if (gioHangs.Count == 0)
            {
                return RedirectToAction("GioHangNull", "GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int id, FormCollection f)
        {
            List<GioHang> gioHangs = LayGioHang();
            GioHang sessiongiohang = gioHangs.SingleOrDefault(n => n.masp == id);
            if (sessiongiohang != null)
            {
                sessiongiohang.soluong = int.Parse(f["Soluong"].ToString());

            }
            return RedirectToAction("Giohang");
        }

        public ActionResult RemoveAll()
        {
            List<GioHang> gioHangs = LayGioHang();
            gioHangs.Clear();
            return RedirectToAction("GioHangNull", "GioHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {

            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "LoginUser");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            List<GioHang> gioHangs = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            //ViewBag.TongHoaDon = TongHoaDon();
            return View(gioHangs);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection collection)
        {
            tbTinhTrangDH tthd = new tbTinhTrangDH();
            tbSanPham sANPHAM = new tbSanPham();
            tbDonHang ddh = new tbDonHang();
            tbChiTietDonHang CTDH = new tbChiTietDonHang();
            tbKhachHang kh = (tbKhachHang)Session["Taikhoan"];
            List<GioHang> gioHangs = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            string DiaChi = collection["DiaChi"];
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.MaTTDH = 1;
            tthd.MaTTDH = (int)ddh.MaTTDH;
            data.tbDonHangs.InsertOnSubmit(ddh);
            //data.tbTinhTrangHoaDons.InsertOnSubmit(tthd);

            //string NguoiNhan = collection["NguoiNhan"];
            ddh.SDT = kh.DienThoaiKH;
            ddh.DiaChi = kh.DiaChiKH;
            string GhiChu = collection["GhiChu"];

            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TongTien = Decimal.Parse(TongTien().ToString());
            //ddh.TongTien = decimal.Parse(TongHoaDon().ToString());
            /*ddh.MaTTHD = '1'*/;
            //dONDATHANG.Dathanhtoan = false;
            data.SubmitChanges();
            foreach (var item in gioHangs)
            {
                tbChiTietDonHang CT = new tbChiTietDonHang();
                tbSanPham SP = new tbSanPham();
                //DONDATHANG dONDATHANG = new DONDATHANG();
                CT.MaDH = ddh.MaDH;
                CT.MaSP = item.masp;
                CT.SoLuong = item.soluong;
                CT.DonGia = (decimal)item.dongia;
                //CT.ThanhTien = (decimal)item.thanhtien;
                //ddh.DiaChi = DiaChi;
                ddh.DiaChi = kh.DiaChiKH;

                //ddh.Email = kh.Email;
                //ddh.NguoiNhan = NguoiNhan;
                ddh.SDT = kh.DienThoaiKH;
                ddh.GhiChu = GhiChu;

                //dONDATHANG.MaVC = Int32.Parse(nvc);
                data.tbChiTietDonHangs.InsertOnSubmit(CT);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }

        public ActionResult GioHangNull()
        {
            return View();
        }
    }
}