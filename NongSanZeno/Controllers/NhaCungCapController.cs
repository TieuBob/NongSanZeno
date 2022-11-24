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
            return View();
        }

        public ActionResult DangKyNCC()
        {
            return View();
        }        
    }
}