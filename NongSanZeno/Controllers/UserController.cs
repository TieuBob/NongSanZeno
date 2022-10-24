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
    public class UserController : Controller
    {
        dbNongSanZenoDataContext data = new dbNongSanZenoDataContext();

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
        // GET: User
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }

        //post: ham dang ky(post) nhap du lieu tu trang dangky
        //va thuc hien viec tao moi du lieu
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, tbKhachHang kh)
        {
            var hoten = collection["Hoten"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Nhaplaimatkhau"];
            var diachi = collection["Diachi"];
            var email = collection["Email"];
            var dienthoai = collection["Dienthoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);

            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống*";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập*";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải nhập mật khẩu*";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu*";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi6"] = "Email không được bỏ trống*";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Phải nhập số điện thoại*";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (KH)
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = MD5Hash(matkhau);
                kh.Email = email;
                kh.DiaChiKH = diachi;
                kh.DienThoaiKH = dienthoai;
                kh.NgaySinh = DateTime.Parse(ngaysinh);
                data.tbKhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }

        public ActionResult Dangnhap(FormCollection collection)
        {
            //Gan cac gia tri nguoi dung nhap lieu cho cac bien
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đang nhập*";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu*";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (KH)
                tbKhachHang kh = data.tbKhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == MD5Hash(matkhau));
                if (kh != null)
                {
                    Session["User"] = kh.HoTen;
                    Session["Taikhoan"] = kh;

                    return RedirectToAction("SanPham", "NongSanZeno");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        public ActionResult Dangxuat()
        {
            Session["User"] = null;
            Session["Taikhoan"] = null;
            return RedirectToAction("SanPham", "NongSanZeno");
        }


        public ActionResult Thongtintk()
        {
            tbKhachHang kh = (tbKhachHang)Session["Taikhoan"];
            return View(kh);
        }
    }
}