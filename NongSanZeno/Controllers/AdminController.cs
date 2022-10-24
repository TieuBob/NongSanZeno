﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NongSanZeno.Models;

namespace NongSanZeno.Controllers
{
    public class AdminController : Controller
    {
        dbNongSanZenoDataContext data = new dbNongSanZenoDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("Index", "NongSanZeno");
            }
            return View();
        }
        [HttpGet]
        public ActionResult DangNhapAD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapAD(FormCollection collection)
        {
            string user = collection["form-username"];
            string pass = collection["form-password"];

            tbAdmin ad = data.tbAdmins.SingleOrDefault(a => a.UserAdmin == user && a.PassAdmin == pass);
            if (ad == null)
            {
                ViewBag.ThongBaoAdmin = "Tài Khoản Hoặc Mật Khẩu Sai";
                return this.DangNhapAD();
            }
            Session["TKadmin"] = ad;
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult QuenMatKhauAD()
        {
            return View();
        }
    }
}