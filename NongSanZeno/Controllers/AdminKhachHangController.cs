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
        public ActionResult DSPhanHoi(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbPhanHoiKHs.OrderByDescending(n => n.STT).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        public ActionResult ChiTietPhanHoi(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
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
        public ActionResult XoaPhanHoi(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
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
        [HttpPost, ActionName("XoaPhanHoi")]
        public ActionResult XacNhanXoaPhanHoi(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
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
                return RedirectToAction("DSPhanHoi"); ;
            }
        }

        //--------------------------- Khách Hàng ----------------------------------
        public ActionResult DSkh(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbKhachHangs.OrderByDescending(n => n.MaKH).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        public ActionResult ChiTietKH(int id, FormCollection collection)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
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
        public ActionResult Xoakh(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
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
        [HttpPost, ActionName("Xoakh")]
        public ActionResult XacNhanXoakh(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "SugarBakery");
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
                return RedirectToAction("DSkh"); ;
            }
        }
    }
}