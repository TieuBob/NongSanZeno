using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using NongSanZeno.Models;
using PagedList;
using System.Configuration;
using System.Data.SqlClient;

namespace NongSanZeno.Controllers
{
    public class AdminDonHangController : Controller
    {
        dbNongSanZenoDataContext data = new dbNongSanZenoDataContext();

        public string ReturnDateForDisplay
        {
            get
            {
                return this.ReturnDateForDisplay.ToString();
            }
        }

        // GET: AdminDonHang
        //--------------------------- Đơn Đặt Hàng ----------------------------------
        [HttpGet]
        public ActionResult DSdonhang(int? page)
        {
            int pagesize = 5;
            int pageNum = (page ?? 1);
            var GioHienTai = DateTime.Today;
            var list = data.tbDonHangs.Where(s => s.NgayDat >= GioHienTai).OrderByDescending(i => i.NgayDat).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        [HttpPost]
        public ActionResult DSdonhang(string date, string date2, int? page)
        {
            int pagesize = 5;
            int pageNum = (page ?? 1);
            var Date = DateTime.Parse(date);

            if (date2 == "")
            {
                var listdate = data.tbDonHangs.Where(s => s.NgayDat >= Date).OrderByDescending(i => i.NgayDat).ToList();
                return View(listdate.ToPagedList(pageNum, pagesize));
            }
            var Date2 = DateTime.Parse(date2);
            var list = data.tbDonHangs.Where(s => s.NgayDat >= Date && s.NgayDat <= Date2).OrderByDescending(i => i.NgayDat).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        public ActionResult ChiTietdonhang(int? id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            if (id == null)
            {
                return HttpNotFound();
            }
            var list = data.tbChiTietDonHangs.Where(s => s.MaDH == id).OrderByDescending(s => s.MaSP).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult Suadonhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }

            tbDonHang dh = data.tbDonHangs.SingleOrDefault(n => n.MaDH == id);
            if (dh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dh);
        }

        [HttpPost, ActionName("Suadonhang")]
        public ActionResult XacNhanSuadonhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbDonHang dh = data.tbDonHangs.SingleOrDefault(n => n.MaDH == id);
                if (dh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                UpdateModel(dh);
                data.SubmitChanges();
                return RedirectToAction("DSdonhang");
            }
        }
        public List<tbChiTietDonHang> LayChiTietdonhang()
        {
            List<tbChiTietDonHang> list = Session["chitietdonhang"] as List<tbChiTietDonHang>;
            if (list == null)
            {
                list = new List<tbChiTietDonHang>();
                Session["chitietdonhang"] = list;
            }
            return list;
        }

        public ActionResult XoaTatCaChiTiet()
        {
            List<tbChiTietDonHang> list = LayChiTietdonhang();
            list.Clear();
            return RedirectToAction("tbDonHang");
        }
        public ActionResult XoaChiTietdonhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbChiTietDonHang ctdh = data.tbChiTietDonHangs.Where(n => n.MaDH == id).FirstOrDefault();
                ViewBag.MaDonHang = ctdh.MaDH;
                if (ctdh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ctdh);
            }
        }
        [HttpPost, ActionName("XoaChiTietdonhang")]
        public ActionResult XacNhanXoaChiTietdonhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbChiTietDonHang ctdh = data.tbChiTietDonHangs.Where(n => n.MaDH == id).FirstOrDefault();

                ViewBag.MaDonHang = ctdh.MaDH;
                if (ctdh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbChiTietDonHangs.DeleteOnSubmit(ctdh);
                data.SubmitChanges();
                return RedirectToAction("DSdonhang", "AdminHoaDon");
            }
        }
        public ActionResult Xoadonhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbDonHang dh = data.tbDonHangs.Where(n => n.MaDH == id).FirstOrDefault();
                // DONDATHANG ncc = context.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
                ViewBag.MaDonHang = dh.MaDH;
                if (dh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(dh);
            }
        }
        [HttpPost, ActionName("Xoadonhang")]
        public ActionResult XacNhanXoadonhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbDonHang dh = data.tbDonHangs.Where(n => n.MaDH == id).FirstOrDefault();

                ViewBag.MaDonHang = dh.MaDH;
                if (dh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbDonHangs.DeleteOnSubmit(dh);
                data.SubmitChanges();
                return RedirectToAction("DSdonhang");
            }
        }
    }
}