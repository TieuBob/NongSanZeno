using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NongSanZeno.Models;
using PagedList;
using PagedList.Mvc;

namespace NongSanZeno.Controllers
{
    public class NongSanZenoController : Controller
    {
        dbNongSanZenoDataContext data = new dbNongSanZenoDataContext();
        // GET: NongSanZeno
        private List<tbSanPham> Laysanpham(int count)
        {
            return data.tbSanPhams.OrderByDescending(a => a.MaSP).Take(count).ToList();
        }
        public ActionResult SanPham(int? page)
        {
            //Tao bien quy dinh so san pham tren moi trang
            int pageSize = 8;
            //tao bien so trang
            int pageNum = (page ?? 1);

            //Lay top 6 san pham ban chay nhat
            var sanphammoi = Laysanpham(30);
            string s = Request.QueryString["s"];
            if (!string.IsNullOrEmpty(s)) sanphammoi = data.tbSanPhams.OrderByDescending(a => a.MaSP).Take(30).Where(w => w.TenSP.Contains(s)).ToList();
            return View(sanphammoi.ToPagedList(pageNum, pageSize));
        }

        //==================== chi tiet san pham ====================
        public ActionResult ChiTiet(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var chitietSP = (from s in data.tbSanPhams where s.MaSP == id select s).Single();
            ViewBag.Description = chitietSP.MoTa;
            return View(chitietSP);
        }

        //==================== tat ca san pham ====================
        public ActionResult TatCaSanPham(int? page)
        {
            var allSP = from sp in data.tbSanPhams select sp;

            int pagesize = 8;
            int pageNum = (page ?? 1);
            return View(allSP.ToPagedList(pageNum, pagesize));
        }

        //==================== san pham theo nhom ====================
        public ActionResult NhomSP()
        {
            var banhkem = from cd in data.tbNhoms select cd;
            return PartialView(banhkem);
        }

        public ActionResult SPTheoNhom(int? id, int? page)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var SPbk = from sp in data.tbSanPhams where sp.MaNhom == id select sp;
            return View(SPbk.ToPagedList(pageNum, pagesize));
        }

        //==================== san pham theo loai ====================
        public ActionResult LoaiSP()
        {
            var loaisp = from cd in data.tbLoaiSanPhams select cd;
            return PartialView(loaisp);
        }

        public ActionResult SPTheoLoai(int? id, int? page)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            int pagesize = 6;
            int pageNum = (page ?? 1);
            var SPLoai = from sp in data.tbSanPhams where sp.MaLoaiSP == id select sp;
            return View(SPLoai.ToPagedList(pageNum, pagesize));
        }
    }
}