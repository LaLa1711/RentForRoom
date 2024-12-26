using RentForRoom.DBContext;
using RentForRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RentForRoom.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        private QLNhaTroEntities db = new QLNhaTroEntities();
        public string GetThanhPho(int? id)
        {
            string html = "<option value=''>----- Chọn Thành Phố -----</option>";
            var lst = db.tbTinhThanhPhoes
                        .Where(tp => tp.Hide != true)
                        .Select(tp => new { tp.IDTP, tp.TenTP })
                        .ToList();

            foreach (var item in lst)
            {
                html += $"<option value='{item.IDTP}'>{item.TenTP}</option>";
            }

            return html;
        }

        public string GetDanhSachQuan(int? idThanhPho, int? idQuan)
        {
            string html = "<option value=''>----- Chọn Quận/Huyện -----</option>";
            if (!idThanhPho.HasValue) return html;

            var lst = db.tbQuanHuyens
                        .Where(qh => qh.Hide != true && qh.IDTP == idThanhPho)
                        .Select(qh => new { qh.IDQuan, qh.TenQuan })
                        .ToList();

            foreach (var item in lst)
            {
                html += $"<option value='{item.IDQuan}'>{item.TenQuan}</option>";
            }

            return html;
        }

        public string GetDanhSachPhuong(int? idQuan, int? idPhuong)
        {
            string html = "<option value=''>----- Chọn Phường/Xã -----</option>";
            if (!idQuan.HasValue) return html;

            var lst = db.tbXaPhuongs
                        .Where(px => px.Hide != true && px.IDQuan == idQuan)
                        .Select(px => new { px.IDPhuong, px.TenPhuong })
                        .ToList();

            foreach (var item in lst)
            {
                html += $"<option value='{item.IDPhuong}'>{item.TenPhuong}</option>";
            }

            return html;
        }
        public ActionResult LoadQuanHuyen(int? idThanhPho)
        {
            if (!idThanhPho.HasValue)
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            var lst = db.tbQuanHuyens
                        .Where(qh => qh.Hide != true && qh.IDTP == idThanhPho.Value)
                        .Select(qh => new
                        {
                            qh.IDQuan,
                            qh.TenQuan
                        })
                        .ToList();

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadXaPhuong(int? idQuan)
        {
            if (!idQuan.HasValue)
            {
                return Json(new List<object>(), JsonRequestBehavior.AllowGet);
            }

            var lst = db.tbXaPhuongs
                        .Where(px => px.Hide != true && px.IDQuan == idQuan.Value)
                        .Select(px => new
                        {
                            px.IDPhuong,
                            px.TenPhuong
                        })
                        .ToList();

            if (!lst.Any())
            {
                Console.WriteLine("Không có phường/xã nào.");
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            ViewBag.ThanhPhoList = GetThanhPho(null);
            ViewBag.QuanList = GetDanhSachQuan(null, null);
            ViewBag.PhuongList = GetDanhSachPhuong(null, null);
            var phong = db.tbChiTietPhongs.Where(b => b.Hide == false).ToList();
            ViewBag.PhongView = phong;

            return PartialView(new PhongModel());
        }
        public ActionResult SearchUsers(string k, string idThanhPho, string idQuan, string idPhuong)
        {
            try
            {
                
                var usersQuery = db.tbChiTietPhongs.AsQueryable();
                if (!string.IsNullOrEmpty(k))
                {
                    usersQuery = usersQuery.Where(r => r.TieuDe.Contains(k) || r.DiaChi.Contains(k));
                }

                if (!string.IsNullOrEmpty(idThanhPho) && int.TryParse(idThanhPho, out int idThanhPhoValue))
                {
                    usersQuery = usersQuery.Where(u => u.IDTP == idThanhPhoValue);
                }
                if (!string.IsNullOrEmpty(idQuan) && int.TryParse(idQuan, out int idQuanValue))
                {
                    usersQuery = usersQuery.Where(u => u.IDQuan == idQuanValue);
                }
                if (!string.IsNullOrEmpty(idPhuong) && int.TryParse(idPhuong, out int idPhuongValue))
                {
                    usersQuery = usersQuery.Where(u => u.IDPhuong == idPhuongValue);
                }
                var filteredUsers = usersQuery.Select(u => new
                {
                    u.MaTaiKhoan,
                    u.TieuDe,
                    u.DiaChi,
                    ThanhPho = db.tbTinhThanhPhoes.FirstOrDefault(t => t.IDTP == u.IDTP) != null
                        ? db.tbTinhThanhPhoes.FirstOrDefault(t => t.IDTP == u.IDTP).TenTP
                        : "Không xác định",
                    Quan = db.tbQuanHuyens.FirstOrDefault(q => q.IDQuan == u.IDQuan) != null
                        ? db.tbQuanHuyens.FirstOrDefault(q => q.IDQuan == u.IDQuan).TenQuan
                        : "Không xác định",
                    Phuong = db.tbXaPhuongs.FirstOrDefault(p => p.IDPhuong == u.IDPhuong) != null
                        ? db.tbXaPhuongs.FirstOrDefault(p => p.IDPhuong == u.IDPhuong).TenPhuong
                        : "Không xác định",
                    u.GiaThue,
                    u.MoTa,
                    u.LinkBai,
                    u.GioGiac,
                    u.LinkMap,
                    u.TienDien,
                    u.TienNuoc,
                    u.TienDichVụ,
                    
                    WC = u.WC.HasValue ? (u.WC.Value ? "chung" : "riêng") : "Không xác định",
                    CuaSoBanCong = u.CuaSoBanCong.HasValue ? (u.CuaSoBanCong.Value ? "Cửa Sổ" : "Ban Công") : "Phòng Kín",
                    KeBep = u.KeBep.HasValue ? (u.KeBep.Value ? "chung" : "riêng") : "Không xác định",
                    Thang = u.Thang.HasValue ? (u.Thang.Value ? "bộ" : "máy") : "Không xác định",
                    MayGiat = u.MayGiat.HasValue ? (u.MayGiat.Value ? "chung" : "riêng") : "Không xác định",
                    ThuCung = u.ThuCung.HasValue ? (u.ThuCung.Value ? "được nuôi" : "không được nuôi") : "Không xác định",
                    TienCoc = u.TienCoc.HasValue ? (u.TienCoc.Value ? "có" : "không") : "Không xác định",
                    u.MayLanh,
                    u.Giuong,
                    u.TuDo,
                    u.Nem,
                    u.TuLanh,
                    u.BanGhe,
                    
                    u.Sofa,
                    u.SoPhongNgu,
                    
                    u.PhuongTien,
                    
                    u.TrangThaiXuLy,
                    u.NoiBat,
                }).ToList();

                if (!filteredUsers.Any())
                {
                    return Json(new { message = "Không tìm thấy phòng nào phù hợp." }, JsonRequestBehavior.AllowGet);
                }

                return PartialView("SearchResults", filteredUsers);
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
                return Json(new { error = "Đã xảy ra lỗi khi xử lý yêu cầu." }, JsonRequestBehavior.AllowGet);
            }
        }


    }
}