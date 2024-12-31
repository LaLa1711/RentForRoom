using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
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
        //public string GetThanhPho(int? id)
        //{
        //    string html = "";

        //    List<ThanhPhoModel> lst = new List<ThanhPhoModel>();
        //    lst = (from grpo in db.tbTinhThanhPhoes
        //           where grpo.Hide != true
        //           select (new ThanhPhoModel
        //           {
        //               Id = grpo.IDTP,
        //               Name = grpo.TenTP
        //           })).ToList();
        //    int tong = lst.Count;

        //    if (id != null)
        //    {
        //        html = "<option value= ''>--- Chọn Thành Phố ---</option>";
        //        for (int i = 0; i < tong; i++)
        //        {
        //            if (id == lst[i].Id)
        //            {
        //                html += "<option selected value='" + lst[i].Id + "'>" + lst[i].Name + "</option>";
        //            }
        //            else
        //            {
        //                html += "<option value='" + lst[i].Id + "'>" + lst[i].Name + "</option>";

        //            }
        //        }
        //    }
        //    else
        //    {
        //        html = "<option selected value= ''>--- Chọn Thành Phố ---</option>";
        //        for (int i = 0; i < tong; i++)
        //        {
        //            html += "<option value='" + lst[i].Id + "'>" + lst[i].Name + "</option>";
        //        }
        //    }
        //    return html;

        //}
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
        public ActionResult SearchUsers(string k, string IDThanhPho, string idQuan, string idPhuong, string p, string st, int page = 1)
        {
            try
            {
                int pageSize = 12; // Số lượng phòng mỗi trang

                // Bắt đầu truy vấn các phòng từ cơ sở dữ liệu
                var usersQuery = db.tbChiTietPhongs
                    .Join(db.tbAlbums.GroupBy(a => a.IDPhong).Select(g => g.FirstOrDefault()), ab => ab.IDPhong, album => album.IDPhong, (ab, album) => new { ab, album.HinhAnh })
                    .Join(db.tbUsers, ab => ab.ab.MaTaiKhoan, user => user.MaTaiKhoan, (ab, u) => new { ab, u.HoTen })
                    .AsQueryable();

                // Bộ lọc theo Thành phố (nếu có IDThanhPho)
                if (!string.IsNullOrEmpty(IDThanhPho) && int.TryParse(IDThanhPho, out int idThanhPhoValue))
                {
                    usersQuery = usersQuery.Where(u => u.ab.ab.IDTP == idThanhPhoValue);
                }

                // Bộ lọc theo từ khóa (nếu có)
                if (!string.IsNullOrEmpty(k))
                {
                    usersQuery = usersQuery.Where(r => r.ab.ab.TieuDe.Contains(k) || r.ab.ab.DiaChi.Contains(k) || (r.HoTen != null && r.HoTen.Contains(k)));
                }


                // Bộ lọc theo Quận/Huyện (nếu có idQuan)
                if (!string.IsNullOrEmpty(idQuan) && int.TryParse(idQuan, out int idQuanValue))
                {
                    usersQuery = usersQuery.Where(u => u.ab.ab.IDQuan == idQuanValue);
                }

                // Bộ lọc theo Phường/Xã (nếu có idPhuong)
                if (!string.IsNullOrEmpty(idPhuong) && int.TryParse(idPhuong, out int idPhuongValue))
                {
                    usersQuery = usersQuery.Where(u => u.ab.ab.IDPhuong == idPhuongValue);
                }

                // Bộ lọc theo khoảng giá (nếu có)
                if (!string.IsNullOrEmpty(p))
                {
                    switch (p)
                    {
                        case "1": usersQuery = usersQuery.Where(r => r.ab.ab.GiaThue < 3000000); break;
                        case "2": usersQuery = usersQuery.Where(r => r.ab.ab.GiaThue >= 3000000 && r.ab.ab.GiaThue <= 5000000); break;
                        case "3": usersQuery = usersQuery.Where(r => r.ab.ab.GiaThue > 5000000 && r.ab.ab.GiaThue <= 7000000); break;
                        case "4": usersQuery = usersQuery.Where(r => r.ab.ab.GiaThue > 7000000); break;
                    }
                }

                // Bộ lọc theo trạng thái (nếu có)
                if (!string.IsNullOrEmpty(st))
                {
                    if (st == "1") usersQuery = usersQuery.Where(r => (bool)r.ab.ab.NoiBat); // Nổi bật
                    if (st == "2") usersQuery = usersQuery.Where(r => !(bool)r.ab.ab.Hide);
                   // if (st == "2") usersQuery = usersQuery.Where(r => r.ab.ab.TrangThaiXuLy == false); // Phòng trống
                }

                // Lấy kết quả đã lọc
                var filteredRooms = usersQuery.Select(r => new PhongModel
                {
                    IDPhong = r.ab.ab.IDPhong,
                    MaTaiKhoan = r.ab.ab.MaTaiKhoan,
                    TieuDe = r.ab.ab.TieuDe,
                    DiaChi = r.ab.ab.DiaChi,
                    IDPhuong = r.ab.ab.IDPhuong,
                    IDQuan = r.ab.ab.IDQuan,
                    IDTP = r.ab.ab.IDTP,
                    GiaThue = (float)r.ab.ab.GiaThue,
                    MoTa = r.ab.ab.MoTa,
                    LinkBai = r.ab.ab.LinkBai,
                    GioGiac = r.ab.ab.GioGiac,
                    LinkMap = r.ab.ab.LinkMap,
                    TienDien = r.ab.ab.TienDien,
                    TienNuoc = r.ab.ab.TienNuoc,
                    TienDichVụ = r.ab.ab.TienDichVụ,
                    CuaSoBanCong = r.ab.ab.CuaSoBanCong,
                    WC = r.ab.ab.WC,
                    KeBep = r.ab.ab.KeBep,
                    MayLanh = r.ab.ab.MayLanh,
                    Giuong = r.ab.ab.Giuong,
                    TuDo = r.ab.ab.TuDo,
                    Nem = r.ab.ab.Nem,
                    TuLanh = r.ab.ab.TuLanh,
                    BanGhe = r.ab.ab.BanGhe,
                    MayGiat = r.ab.ab.MayGiat,
                    Sofa = r.ab.ab.Sofa,
                    SoPhongNgu = r.ab.ab.SoPhongNgu,
                    PhuongTien = r.ab.ab.PhuongTien,
                    ThuCung = r.ab.ab.ThuCung,
                    TienCoc = r.ab.ab.TienCoc,
                    Hide = r.ab.ab.Hide,
                    NoiBat = r.ab.ab.NoiBat,
                    TrangThaiXuLy = r.ab.ab.TrangThaiXuLy,
                    HinhAnh = r.ab.HinhAnh
                }).ToList();

                // Tính số trang
                int totalProducts = filteredRooms.Count();
                int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
                var paginatedRooms = filteredRooms.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                // Truyền dữ liệu phân trang vào View
                ViewBag.TotalPages = totalPages;
                ViewBag.CurrentPage = page;

                // Trả về View với dữ liệu đã phân trang
                return View(paginatedRooms);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                return Json(new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." }, JsonRequestBehavior.AllowGet);
            }
        }

        //public ActionResult SearchUsers(string k, string IDThanhPho, string idQuan, string idPhuong, string p, string st, int page = 1)
        //{
        //    try
        //    {
        //        int pageSize = 12; // Kích thước mỗi trang
        //        var usersQuery = (from ab in db.tbChiTietPhongs
        //                          join album in
        //                              (from a in db.tbAlbums
        //                               group a by a.IDPhong into g
        //                               select g.FirstOrDefault())
        //                          on ab.IDPhong equals album.IDPhong into albums
        //                          from album in albums.DefaultIfEmpty()
        //                          join user in db.tbUsers on ab.MaTaiKhoan equals user.MaTaiKhoan into userJoin
        //                          from u in userJoin.DefaultIfEmpty()  // Join với bảng tbUser
        //                          select new
        //                          {
        //                              ab,           // Thông tin phòng
        //                              album.HinhAnh, // Hình ảnh album
        //                              u.HoTen       // Tên người dùng từ bảng tbUser
        //                          }).AsQueryable();

        //        // Bộ lọc theo từ khóa
        //        if (!string.IsNullOrEmpty(k))
        //        {
        //            usersQuery = usersQuery.Where(r => r.ab.TieuDe.Contains(k) ||
        //                                                r.ab.DiaChi.Contains(k) ||
        //                                                (r.HoTen != null && r.HoTen.Contains(k))); // Kiểm tra tên người dùng (host)
        //        }

        //        // Bộ lọc theo Thành phố
        //        if (!string.IsNullOrEmpty(IDThanhPho) && int.TryParse(IDThanhPho, out int idThanhPhoValue))
        //        {
        //            usersQuery = usersQuery.Where(u => u.ab.IDTP == idThanhPhoValue);
        //        }

        //        // Bộ lọc theo Quận/Huyện chỉ khi có idQuan
        //        if (!string.IsNullOrEmpty(idQuan) && int.TryParse(idQuan, out int idQuanValue))
        //        {
        //            usersQuery = usersQuery.Where(u => u.ab.IDQuan == idQuanValue);
        //        }

        //        // Bộ lọc theo Phường/Xã chỉ khi có idPhuong
        //        if (!string.IsNullOrEmpty(idPhuong) && int.TryParse(idPhuong, out int idPhuongValue))
        //        {
        //            usersQuery = usersQuery.Where(u => u.ab.IDPhuong == idPhuongValue);
        //        }


        //        // Bộ lọc theo khoảng giá
        //        if (!string.IsNullOrEmpty(p))
        //        {
        //            switch (p)
        //            {
        //                case "1": // Dưới 3 triệu
        //                    usersQuery = usersQuery.Where(r => r.ab.GiaThue < 3000000);
        //                    break;
        //                case "2": // Từ 3 triệu đến 5 triệu
        //                    usersQuery = usersQuery.Where(r => r.ab.GiaThue >= 3000000 && r.ab.GiaThue <= 5000000);
        //                    break;
        //                case "3": // Từ 5 triệu đến 7 triệu
        //                    usersQuery = usersQuery.Where(r => r.ab.GiaThue > 5000000 && r.ab.GiaThue <= 7000000);
        //                    break;
        //                case "4": // Trên 7 triệu
        //                    usersQuery = usersQuery.Where(r => r.ab.GiaThue > 7000000);
        //                    break;
        //            }
        //        }

        //        // Bộ lọc theo trạng thái
        //        if (!string.IsNullOrEmpty(st))
        //        {
        //            switch (st)
        //            {
        //                case "1": // Nổi bật
        //                    usersQuery = usersQuery.Where(r => r.ab.NoiBat == true);
        //                    break;
        //                case "2": // Phòng trống
        //                    usersQuery = usersQuery.Where(r => r.ab.TrangThaiXuLy == true);
        //                    break;
        //            }
        //        }

        //        // Lấy kết quả đã lọc
        //        var filteredRooms = usersQuery.Select(r => new PhongModel
        //        {
        //            IDPhong = r.ab.IDPhong,
        //            MaTaiKhoan = r.ab.MaTaiKhoan,
        //            TieuDe = r.ab.TieuDe,
        //            DiaChi = r.ab.DiaChi,
        //            IDPhuong = r.ab.IDPhuong,
        //            IDQuan = r.ab.IDQuan,
        //            IDTP = r.ab.IDTP,
        //            GiaThue = (float)r.ab.GiaThue,
        //            MoTa = r.ab.MoTa,
        //            LinkBai = r.ab.LinkBai,
        //            GioGiac = r.ab.GioGiac,
        //            LinkMap = r.ab.LinkMap,
        //            TienDien = r.ab.TienDien,
        //            TienNuoc = r.ab.TienNuoc,
        //            TienDichVụ = r.ab.TienDichVụ,
        //            CuaSoBanCong = r.ab.CuaSoBanCong,
        //            WC = r.ab.WC,
        //            KeBep = r.ab.KeBep,
        //            MayLanh = r.ab.MayLanh,
        //            Giuong = r.ab.Giuong,
        //            TuDo = r.ab.TuDo,
        //            Nem = r.ab.Nem,
        //            TuLanh = r.ab.TuLanh,
        //            BanGhe = r.ab.BanGhe,
        //            MayGiat = r.ab.MayGiat,
        //            Sofa = r.ab.Sofa,
        //            SoPhongNgu = r.ab.SoPhongNgu,
        //            PhuongTien = r.ab.PhuongTien,
        //            ThuCung = r.ab.ThuCung,
        //            TienCoc = r.ab.TienCoc,
        //            Hide = r.ab.Hide,
        //            NoiBat = r.ab.NoiBat,
        //            TrangThaiXuLy = r.ab.TrangThaiXuLy,
        //            HinhAnh = r.HinhAnh
        //        }).ToList();

        //        // Tính số trang
        //        int totalProducts = filteredRooms.Count(); // Tổng số phòng đã lọc
        //        int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize); // Số trang

        //        // Phân trang: chọn phòng trong phạm vi trang hiện tại
        //        var paginatedRooms = filteredRooms.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        //        // Truyền dữ liệu phân trang vào View
        //        ViewBag.TotalPages = totalPages;
        //        ViewBag.CurrentPage = page;

        //        // Trả về kết quả View
        //        return View(paginatedRooms);  // Trả về View với dữ liệu tìm kiếm
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return Json(new { success = false, message = "Đã xảy ra lỗi khi xử lý yêu cầu." }, JsonRequestBehavior.AllowGet);
        //    }
        //}

    }
}