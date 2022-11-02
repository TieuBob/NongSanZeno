using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NongSanZeno.Models;

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
        public ActionResult purchase()
        {
            return View();
        }
    }
}