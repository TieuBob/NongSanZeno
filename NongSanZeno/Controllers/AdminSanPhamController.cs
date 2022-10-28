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
    public class AdminSanPhamController : Controller
    {
        dbNongSanZenoDataContext data = new dbNongSanZenoDataContext();
        // GET: AdminSanPham
        //------------------------------ Sản Phẩm ---------------------------------------vinh

        public ActionResult DSsanpham(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbSanPhams.OrderByDescending(s => s.MaSP).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        public ActionResult Loaisp(int id)
        {
            var list = data.tbLoaiSanPhams.Where(n => n.MaLoaiSP == id);
            return View(list.Single());
        }
        public ActionResult Nhomsp(int id)
        {
            var list = data.tbNhoms.Where(n => n.MaNhom == id);
            return View(list.Single());
        }
        public ActionResult Nhacungcap(int id)
        {
            var list = data.tbNhaCungCaps.Where(n => n.MaNCC == id);
            return View(list.Single());
        }
        public ActionResult Donvitinh(int id)
        {
            var list = data.tbDonViTinhs.Where(n => n.MaDVT == id);
            return View(list.Single());
        }


        [HttpGet]
        public ActionResult Themsanpham()
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            ViewBag.Loai = new SelectList(data.tbLoaiSanPhams.ToList().OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaBK = new SelectList(data.tbNhoms.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom");
            ViewBag.MaDVT = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.MaDVT), "MaDVT", "TenDVT");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themsanpham(tbSanPham sp, FormCollection collection, HttpPostedFileBase fileUpload)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            ViewBag.Loai = new SelectList(data.tbLoaiSanPhams.ToList().OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP");
            ViewBag.MaBK = new SelectList(data.tbNhoms.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom");
            ViewBag.MaDVT = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.MaDVT), "MaDVT", "TenDVT");
            var tensp = collection["Ten"];
            var gia = collection["Gia"];
            var mota = collection["textarea"];
            var date = collection["Date"];
            var loai = collection["Loai"];
            var nhom = collection["MaBK"];
            var dvt = collection["Dvt"];

            var filename = Path.GetFileName(fileUpload.FileName);
            var path = Path.Combine(Server.MapPath("~/images/sanpham"), filename);
            if (System.IO.File.Exists(path))
            {
                ViewBag.ThongBaoAnh = "Hình Ảnh Đã Tồn Tại";
                return View();
            }
            else
            {
                fileUpload.SaveAs(path);
            }

            sp.TenSP = tensp;
            sp.GiaBan = decimal.Parse(gia);
            sp.MoTa = mota;
            sp.NgayCapNhat = DateTime.Parse(date);
            sp.MaLoaiSP = Int32.Parse(loai);
            sp.MaNhom = Int32.Parse(nhom);
            sp.MaDVT = Int32.Parse(dvt);
            sp.AnhSP = filename;
            data.tbSanPhams.InsertOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("DSsanpham", "AdminSanPham");
        }

        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbSanPham sp = data.tbSanPhams.SingleOrDefault(n => n.MaSP == id);
                ViewBag.MaSP = sp.MaSP;
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sp);
            }
        }
        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult XacNhanXoasanpham(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbSanPham sp = data.tbSanPhams.SingleOrDefault(n => n.MaSP == id);
                ViewBag.MaSP = sp.MaSP;
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbSanPhams.DeleteOnSubmit(sp);
                data.SubmitChanges();
                return RedirectToAction("DSsanpham");
            }
        }


        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            tbSanPham sp = data.tbSanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.Loai = new SelectList(data.tbLoaiSanPhams.ToList().OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP", sp.MaLoaiSP);
            ViewBag.MaBK = new SelectList(data.tbNhoms.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom", sp.MaNhom);
            ViewBag.MaDVT = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.MaDVT), "MaDVT", "TenDVT", sp.MaDVT);

            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        [HttpPost, ActionName("Suasanpham")]
        public ActionResult XacNhanSuasanpham(FormCollection collection, int id, HttpPostedFileBase fileUpload)
        {
            var img = "";
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            if (fileUpload != null)
            {
                img = Path.GetFileName(fileUpload.FileName);
                var path = Path.Combine(Server.MapPath("~/images/sanpham"), img);
                if (!System.IO.File.Exists(path))//Sản Phẩm Chưa Tồn Tại
                {
                    fileUpload.SaveAs(path);
                }
            }
            else
            {
                img = collection["Anh"];
            }
            tbSanPham sp = data.tbSanPhams.SingleOrDefault(n => n.MaSP == id);
            sp.AnhSP = img;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(sp);
            data.SubmitChanges();
            return RedirectToAction("DSsanpham");

        }

        public ActionResult Chitietsanpham(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }


            tbSanPham ncc = data.tbSanPhams.SingleOrDefault(n => n.MaSP == id);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }

        //----------------------------------- Loại Sản Phẩm ------------------------------------
        public ActionResult DSloaisanpham(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbLoaiSanPhams.OrderByDescending(s => s.MaLoaiSP).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        [HttpGet]
        public ActionResult Themlsp()
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            ViewBag.MaLoaiSP = new SelectList(data.tbLoaiSanPhams.ToList().OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP", "MoTa");
            return View();
        }
        [HttpPost]
        public ActionResult Themlsp(tbLoaiSanPham lsp, FormCollection collection, HttpPostedFileBase fileUpload)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            ViewBag.MaLoaiSP = new SelectList(data.tbLoaiSanPhams.ToList().OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP", "MoTa");
            if (ModelState.IsValid)
            {
                data.tbLoaiSanPhams.InsertOnSubmit(lsp);
                data.SubmitChanges();
            }
            return RedirectToAction("DSloaisanpham", "AdminSanPham");
        }

        public ActionResult Sualsp(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            tbLoaiSanPham lsp = data.tbLoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
            ViewBag.MaLoaiSP = new SelectList(data.tbLoaiSanPhams.ToList().OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoaiSP", "MoTa", lsp.MaLoaiSP);
            if (lsp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(lsp);
        }

        [HttpPost, ActionName("Sualsp")]
        public ActionResult XacNhanSualsp(FormCollection collection, int id)
        {
            var img = "";
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }

            tbLoaiSanPham lsp = data.tbLoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);

            if (lsp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(lsp);
            data.SubmitChanges();
            return RedirectToAction("DSloaisanpham");
        }

        [HttpGet]
        public ActionResult Xoalsp(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbLoaiSanPham lsp = data.tbLoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);

                ViewBag.MaLoaiSP = lsp.MaLoaiSP;
                if (lsp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(lsp);
            }
        }
        [HttpPost, ActionName("Xoalsp")]
        public ActionResult XacNhanXoalsp(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbLoaiSanPham lsp = data.tbLoaiSanPhams.SingleOrDefault(n => n.MaLoaiSP == id);
                ViewBag.MaLoaiSP = lsp.MaLoaiSP;
                if (lsp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbLoaiSanPhams.DeleteOnSubmit(lsp);
                data.SubmitChanges();
                return RedirectToAction("DSloaisanpham");
            }
        }



        //----------------------------------- Nhóm Sản Phẩm ------------------------------------
        public ActionResult DSnhomsanpham(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbNhoms.OrderByDescending(s => s.MaNhom).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }

        [HttpGet]
        public ActionResult Themnsp()
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            ViewBag.MaBK = new SelectList(data.tbNhoms.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom");
            return View();
        }
        [HttpPost]
        public ActionResult Themnsp(tbNhom bk, FormCollection collection, HttpPostedFileBase fileUpload)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            ViewBag.MaBK = new SelectList(data.tbNhoms.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom");
            if (ModelState.IsValid)
            {
                data.tbNhoms.InsertOnSubmit(bk);
                data.SubmitChanges();
            }
            return RedirectToAction("DSnhomsanpham", "AdminSanPham");
        }

        public ActionResult Suansp(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            tbNhom nsp = data.tbNhoms.SingleOrDefault(n => n.MaNhom == id);
            ViewBag.MaBK = new SelectList(data.tbNhoms.ToList().OrderBy(n => n.TenNhom), "MaNhom", "TenNhom", nsp.MaNhom);
            if (nsp == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(nsp);
        }

        [HttpPost, ActionName("Suansp")]
        public ActionResult XacNhanSuansp(FormCollection collection, int id)
        {
            var img = "";
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }

            tbNhom nsp = data.tbNhoms.SingleOrDefault(n => n.MaNhom == id);

            if (nsp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(nsp);
            data.SubmitChanges();
            return RedirectToAction("DSnhomsanpham");
        }

        [HttpGet]
        public ActionResult Xoansp(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbNhom nsp = data.tbNhoms.SingleOrDefault(n => n.MaNhom == id);

                ViewBag.MaBK = nsp.MaNhom;
                if (nsp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(nsp);
            }
        }
        [HttpPost, ActionName("Xoansp")]
        public ActionResult XacNhanXoansp(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbNhom nsp = data.tbNhoms.SingleOrDefault(n => n.MaNhom == id);
                ViewBag.MaBK = nsp.MaNhom;
                if (nsp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbNhoms.DeleteOnSubmit(nsp);
                data.SubmitChanges();
                return RedirectToAction("DSnhomsanpham");
            }
        }



        //-----------------------------------Nhà Cung Cấp---------------------------------------
        public ActionResult DSnhacungcap(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbNhaCungCaps.OrderByDescending(s => s.MaNCC).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        [HttpGet]
        public ActionResult ThemNcc()
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            ViewBag.MaNCC = new SelectList(data.tbNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", "DiaChi");
            return View();
        }
        [HttpPost]
        public ActionResult ThemNcc(tbNhaCungCap ncc)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            ViewBag.MaNCC = new SelectList(data.tbNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", "DiaChi");
            if (ModelState.IsValid)
            {
                data.tbNhaCungCaps.InsertOnSubmit(ncc);
                data.SubmitChanges();
            }
            return RedirectToAction("DSnhacungcap", "AdminSanPham");
        }

        [HttpGet]
        public ActionResult XoaNcc(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbNhaCungCap ncc = data.tbNhaCungCaps.SingleOrDefault(n => n.MaNCC == id);

                ViewBag.MaNCC = ncc.MaNCC;
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ncc);
            }
        }
        [HttpPost, ActionName("XoaNcc")]
        public ActionResult XacNhanXoaNcc(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbNhaCungCap ncc = data.tbNhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
                ViewBag.MaNCC = ncc.MaNCC;
                if (ncc == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbNhaCungCaps.DeleteOnSubmit(ncc);
                data.SubmitChanges();
                return RedirectToAction("DSnhacungcap");
            }
        }

        [HttpGet]
        public ActionResult SuaNcc(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            tbNhaCungCap ncc = data.tbNhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            ViewBag.MaNCC = new SelectList(data.tbNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaBK", "TenBK", ncc.MaNCC);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(ncc);
        }

        [HttpPost, ActionName("SuaNcc")]
        public ActionResult XacNhanSuaNcc(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }

            tbNhaCungCap ncc = data.tbNhaCungCaps.SingleOrDefault(n => n.MaNCC == id);

            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(ncc);
            data.SubmitChanges();
            return RedirectToAction("DSnhacungcap");
        }



        //----------------------------------- Đơn Vị Tính ---------------------------------------
        public ActionResult DSdonvitinh(int? page)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            int pagesize = 8;
            int pageNum = (page ?? 1);
            var list = data.tbDonViTinhs.OrderByDescending(s => s.MaDVT).ToList();
            return View(list.ToPagedList(pageNum, pagesize));
        }
        [HttpGet]
        public ActionResult ThemDvt()
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            ViewBag.MaDVT = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.TenDVT), "MaDVT", "TenDVT", "MoTa");
            return View();
        }
        [HttpPost]
        public ActionResult ThemDvt(tbDonViTinh dvt)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            ViewBag.MaDVT = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.TenDVT), "MaDVT", "TenDVT", "MoTa");
            if (ModelState.IsValid)
            {
                data.tbDonViTinhs.InsertOnSubmit(dvt);
                data.SubmitChanges();
            }
            return RedirectToAction("DSdonvitinh", "AdminSanPham");
        }

        public ActionResult Suadvt(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            tbDonViTinh dvt = data.tbDonViTinhs.SingleOrDefault(n => n.MaDVT == id);
            ViewBag.MaBK = new SelectList(data.tbDonViTinhs.ToList().OrderBy(n => n.TenDVT), "MaDVT", "TenDVT", "MoTa", dvt.MaDVT);
            if (dvt == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(dvt);
        }

        [HttpPost, ActionName("Suadvt")]
        public ActionResult XacNhanSuadvt(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }

            tbDonViTinh dvt = data.tbDonViTinhs.SingleOrDefault(n => n.MaDVT == id);

            if (dvt == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            UpdateModel(dvt);
            data.SubmitChanges();
            return RedirectToAction("DSdonvitinh");
        }

        [HttpGet]
        public ActionResult Xoadvt(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbDonViTinh dvt = data.tbDonViTinhs.SingleOrDefault(n => n.MaDVT == id);

                ViewBag.MaBK = dvt.MaDVT;
                if (dvt == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(dvt);
            }
        }
        [HttpPost, ActionName("Xoadvt")]
        public ActionResult XacNhanXoadvt(int id)
        {
            if (Session["TKadmin"] == null)
            {
                return RedirectToAction("SanPham", "NongSanZeno");
            }
            else
            {
                tbDonViTinh dvt = data.tbDonViTinhs.SingleOrDefault(n => n.MaDVT == id);
                ViewBag.MaDVT = dvt.MaDVT;
                if (dvt == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                data.tbDonViTinhs.DeleteOnSubmit(dvt);
                data.SubmitChanges();
                return RedirectToAction("DSdonvitinh");
            }
        }
    }
}