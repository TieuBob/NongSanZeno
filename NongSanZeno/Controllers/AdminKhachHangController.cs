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
    public class AdminKhachHangController : Controller
    {
        dbNongSanZenoDataContext data = new dbNongSanZenoDataContext();

        // GET: AdminKhachHang
        //------------------------------ Phản  Hồi Khách Hàng ------------------------------------
        public ActionResult DSphanhoi(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbPhanHoiKHs.OrderByDescending(n => n.STT).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        public ActionResult ChiTietphanhoi(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            tbPhanHoiKH ph = data.tbPhanHoiKHs.SingleOrDefault(n => n.STT == id);
            if (ph == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ph);
        }
        [HttpGet]
        public ActionResult Xoaphanhoi(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbPhanHoiKH ph = data.tbPhanHoiKHs.SingleOrDefault(n => n.STT == id);
                ViewBag.STT = ph.STT;
                if (ph == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ph);
            }
        }
        [HttpPost, ActionName("Xoaphanhoi")]
        public ActionResult XacNhanXoaphanhoi(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbPhanHoiKH ph = data.tbPhanHoiKHs.SingleOrDefault(n => n.STT == id);
                ViewBag.STT = ph.STT;
                if (ph == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbPhanHoiKHs.DeleteOnSubmit(ph);
                data.SubmitChanges();
                return RedirectToAction("DSphanhoi"); ;
            }
        }

        //--------------------------- Khách Hàng ----------------------------------
        public ActionResult DSkhachhang(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbKhachHangs.OrderByDescending(n => n.MaKH).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        public ActionResult ChiTietkhachhang(int id, FormCollection collection)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            tbKhachHang kh = data.tbKhachHangs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpGet]
        public ActionResult Xoakhachhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbKhachHang kh = data.tbKhachHangs.SingleOrDefault(n => n.MaKH == id);
                ViewBag.MaKH = kh.MaKH;
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(kh);
            }
        }
        [HttpPost, ActionName("Xoakhachhang")]
        public ActionResult XacNhanXoakhachhang(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbKhachHang kh = data.tbKhachHangs.SingleOrDefault(n => n.MaKH == id);
                ViewBag.MaKH = kh.MaKH;
                if (kh == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbKhachHangs.DeleteOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DSkhachhang"); ;
            }
        }
    }
}