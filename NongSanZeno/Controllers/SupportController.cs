using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NongSanZeno.Controllers
{
    public class SupportController : Controller
    {
        // GET: Support
        public ActionResult Help()
        {
            return View();
        }

        public ActionResult MuaSam()
        {
            return View();
        }

        public ActionResult ThanhToan()
        {
            return View();
        }

        public ActionResult TraHangVaHoanTien()
        {
            return View();
        }
        public ActionResult ThongTinChung()
        {
            return View();
        }

        public ActionResult KhuyenMaiVaUuDai()
        {
            return View();
        }

        public ActionResult DonHangVaVanChuyen()
        {
            return View();
        }

        public ActionResult NguoiBanVaDoiTac()
        {
            return View();
        }
    }
}