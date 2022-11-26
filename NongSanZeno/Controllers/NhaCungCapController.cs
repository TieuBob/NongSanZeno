using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NongSanZeno.Controllers
{
    public class NhaCungCapController : Controller
    {
        // GET: NhaCungCap
        public ActionResult ChaoMungNCC()
        {
            //if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            //{
            //    return RedirectToAction("Dangnhap", "LoginUser");
            //}
            return View();
        }

        public ActionResult ThongTin()
        {
            return View();
        }

        public ActionResult VanChuyen()
        {
            return View();
        }

        public ActionResult CungCapSanPham()
        {
            return View();
        }
    }
}