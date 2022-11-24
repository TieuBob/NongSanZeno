using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NongSanZeno.Models;

using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Net.Mail;
using PagedList;
using System.Configuration;
using System.Data.SqlClient;

namespace NongSanZeno.Controllers
{
    public class userController : Controller
    {
        // create string MD5
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        dbNongSanZenoDataContext data = new dbNongSanZenoDataContext();

        public string ReturnDateForDisplay
        {
            get
            {
                return this.ReturnDateForDisplay.ToString();
            }
        }
        // GET: user
        public ActionResult profile()
        {
            tbKhachHang kh = (tbKhachHang)Session["Taikhoan"];
            return View(kh);
        }
        public ActionResult password()
        {
            return View();
        }
        public ActionResult address()
        {
            return View();
        }
        public ActionResult purchase(int? page)
        {
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var GioHienTai = DateTime.Today;
            var list = data.tbDonHangs.Where(s => s.NgayDat >= GioHienTai).OrderByDescending(i => i.NgayDat).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        [HttpPost]
        public ActionResult purchase(string date, string date2, int? page)
        {
            int pagesize = 8;
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
    }
}