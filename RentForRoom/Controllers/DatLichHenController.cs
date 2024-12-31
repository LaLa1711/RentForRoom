using RentForRoom.DBContext;
using RentForRoom.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Data.SqlTypes;

namespace RentForRoom.Controllers
{
    public class DatLichHenController : Controller
    {
        // GET: DatLichHen
        private QLNhaTroEntities db = new QLNhaTroEntities();
        public ActionResult Index()
        {
            try
            {
                DatLichModel ban = new DatLichModel();
                ban = (from ab in db.tbDatLiches
                       join qq in db.tbChiTietPhongs on ab.IDPhong equals qq.IDPhong into qqs
                       from qq in qqs.DefaultIfEmpty()
                       select (new DatLichModel
                       {
                           IDPhong = qq.IDPhong,
                           TieuDe = qq.TieuDe,
                           DiaChi = qq.DiaChi,
                           IDPhuong = qq.IDPhuong,
                           IDQuan = qq.IDQuan,
                           IDTP = qq.IDTP,
                           GiaThue = (float?)qq.GiaThue,
                           
                       })).FirstOrDefault();
                return PartialView(ban);
            }
            catch (Exception ex)
            {
                return Redirect("/not-found");
            }
        }
        [HttpPost]
        public JsonResult GetImages(int roomid)
        {
            using (var context = new QLNhaTroEntities())
            {
                // Truy vấn tất cả hình ảnh thuộc roomid
                var images = context.tbAlbums
                    .Where(p => p.IDPhong == roomid)
                    .Select(p => p.HinhAnh) // Giả sử bạn lưu tên hình ảnh trong cột HinhAnh
                    .ToList();

                if (images.Any())
                {
                    // Trả về các hình ảnh (có thể cần thêm URL gốc nếu HinhAnh là tên file)
                    return Json(new { success = true, images = images });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
        }




        [HttpPost]
        public ActionResult Index(DatLichModel tbCon)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DateTime? ngayXemPhong = null;
                    if (!string.IsNullOrEmpty(tbCon.NgayXemPhong) && DateTime.TryParse(tbCon.NgayXemPhong, out DateTime parsedNgayXemPhong))
                    {
                        ngayXemPhong = parsedNgayXemPhong;
                    }
                    var contact = new tbDatLich
                    {
                        IDPhong = tbCon.IDPhong,                         
                        HoTen = tbCon.HoTen,
                        SDT = tbCon.SDT,
                        Email = tbCon.Email,
                        ThuCung = tbCon.ThuCung,
                        SoLuongXe = tbCon.SoLuongXe,
                        GhiChu = tbCon.GhiChu,
                        NgayXemPhong = ngayXemPhong,
                        NgayChuyenVao = tbCon.NgayChuyenVao,
                        Hide = false
                    };
                    db.tbDatLiches.Add(contact);
                    db.SaveChanges();
                    return Redirect("/Home/Index");
                }
                else
                {
                    return PartialView(tbCon);
                }
            }
            catch
            {
                return PartialView(tbCon);
            }

        }
        
        [HttpPost]
        public JsonResult SaveBooking(DatLichModel model)
        {
            try
            {
                DateTime? ngayXemPhong = null;
                if (!string.IsNullOrEmpty(model.NgayXemPhong) && DateTime.TryParse(model.NgayXemPhong, out DateTime parsedNgayXemPhong))
                {
                    ngayXemPhong = parsedNgayXemPhong;
                }
                var newBooking = new tbDatLich
                {
                    IDPhong = model.IDPhong,
                    SoNguoiO = model.SoNguoiO,
                    NgayXemPhong = ngayXemPhong,
                    GhiChu = model.GhiChu,
                    NgayChuyenVao = model.NgayChuyenVao,
                    HoTen = model.HoTen,
                    SDT = model.SDT,
                    Email = model.Email,
                    ThuCung = model.ThuCung,
                    SoLuongXe = model.SoLuongXe,
                    Hide = false
                };
                db.tbDatLiches.Add(newBooking);
                db.SaveChanges();
                var phong = db.tbChiTietPhongs
                      .Where(p => p.IDPhong == model.IDPhong)
                      .FirstOrDefault();

                if (phong != null)
                {
                    var phuong = db.tbXaPhuongs.FirstOrDefault(p => p.IDPhuong == phong.IDPhuong);
                    var quan = db.tbQuanHuyens.FirstOrDefault(q => q.IDQuan == phong.IDQuan);
                    var tp = db.tbTinhThanhPhoes.FirstOrDefault(t => t.IDTP == phong.IDTP);
                    var host = db.tbUsers.FirstOrDefault(a => a.MaTaiKhoan == phong.MaTaiKhoan);
                    SendNewPasswordEmail(model.Email, host?.HoTen, phong.IDPhong.ToString(), phong.GiaThue.ToString(), phong.DiaChi, phuong?.TenPhuong, quan?.TenQuan, tp?.TenTP, ngayXemPhong);
                    SendEmailToHost(host?.Gmail, model.Email, model.SDT, model.HoTen, phong.IDPhong.ToString(), phong.GiaThue.ToString(), phong.DiaChi, phuong?.TenPhuong, quan?.TenQuan, tp?.TenTP, ngayXemPhong, model.NgayChuyenVao, model.SoNguoiO, model.SoLuongXe, model.ThuCung, model.GhiChu);
                }

                return Json(new { Success = true, Message = "Đặt lịch hẹn thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Message = "Đã có lỗi xảy ra. Vui lòng thử lại!" });
            }
        }

        private void SendNewPasswordEmail(string toEmail, string maphong, string host, string giathue, string diachi, string tenPhuong, string tenQuan, string tenTP, DateTime? ngayxemphong)
        {
            SendMail("LaLaHome", "XÁC NHẬN LỊCH HẸN XEM PHÒNG", maphong, host, giathue, diachi, tenPhuong, tenQuan, tenTP, ngayxemphong, toEmail, "cqt17112003@gmail.com");

        }
        public bool SendMail(string name, string subject, string host, string maphong, string giathue, string diachi, string tenPhuong, string tenQuan, string tenTP, DateTime? ngayxemphong,
           string toMail, string formmail)
        {
            bool rs = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com"; //host name
                    smtp.Port = 587; //port number
                    smtp.EnableSsl = true; //whether your smtp server requires SSL
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential()
                    {
                        UserName = formmail,
                        Password = "svzb wljh lnfe pzja"
                    };
                }
                MailAddress fromAddress = new MailAddress(formmail, name);
                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                string imageUrl = this.Server.MapPath("/Content/img/lala.jpg");

                LinkedResource inlineLogo = new LinkedResource(imageUrl, "image/jpeg");
                inlineLogo.ContentId = "hostImage";
                string  body = $@"
            <h2><strong>Cảm ơn bạn đã đặt lịch xem phòng tại LaLaHome.</strong></h2>
            <p><strong>Mã phòng:</strong> P{maphong}</p>
            <p><strong>Giá thuê:</strong> {giathue}</p>
            <p><strong>Địa chỉ:</strong> {diachi}, {tenPhuong}, {tenQuan}, {tenTP}</p>
            <p><strong>Ngày xem phòng:</strong> {ngayxemphong?.ToString("dd/MM/yyyy HH:mm")}</p>
            <p></p>
            <p>Chủ nhà <strong>{host}</strong> sẽ liên hệ bạn trong thời gian sớm nhất.</p>
            <p>Bạn hãy chú ý điện thoại và đến đúng hẹn!</p>
            <p>Chúng tôi rất mong được đón tiếp bạn tại LaLaHome!</p>
            <p><strong>Liên hệ Hotline LaLaHome: 090.661.3087 nếu cần hỗ trợ. Xin cảm ơn!</strong></p>";

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                avHtml.LinkedResources.Add(inlineLogo);
                message.AlternateViews.Add(avHtml);
                smtp.Send(message);
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                rs = false;
            }
            return rs;
        }



        private void SendEmailToHost(string hostEmail, string email, string SDT, string guestName, string maphong, string giathue, string diachi, string tenPhuong, string tenQuan, string tenTP, DateTime? ngayxemphong, DateTime? ngaychuyenvao, int? songuoio, int? soluongxe, bool? thucung, string ghichu)
        {
            if (!string.IsNullOrEmpty(hostEmail))
            {
                string ngaychuyenvaoStr = ngaychuyenvao.HasValue ? ngaychuyenvao.Value.ToString("dd/MM/yyyy") : "Không rõ";
                string soluongxeStr = soluongxe.HasValue ? soluongxe.Value.ToString() : "Không rõ";
                string thucungStr = thucung.HasValue ? (thucung.Value ? "Có" : "Không") : "Không rõ";

                SendMail1("LaLaHome", "CÓ LỊCH HẸN XEM PHÒNG MỚI", email, guestName, SDT, maphong, giathue, diachi, tenPhuong, tenQuan, tenTP, ngayxemphong, ngaychuyenvaoStr, songuoio?.ToString(), soluongxeStr, thucungStr, ghichu, hostEmail, "cqt17112003@gmail.com");
            }
        }


        public bool SendMail1(string name, string subject, string email, string guestName, string SDT, string maphong, string giathue, string diachi, string tenPhuong, string tenQuan, string tenTP, DateTime? ngayxemphong, string ngaychuyenvao, string songuoio, string soluongxe, string thucung, string ghichu, string toMail, string formmail)
        {
            bool rs = false;
            try
            {
                MailMessage message = new MailMessage();
                var smtp = new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential()
                    {
                        UserName = formmail,
                        Password = "svzb wljh lnfe pzja"
                    }
                };

                MailAddress fromAddress = new MailAddress(formmail, name);
                message.From = fromAddress;
                message.To.Add(toMail);
                message.Subject = subject;
                message.IsBodyHtml = true;
                string imageUrl = this.Server.MapPath("/Content/img/lala.jpg");

                LinkedResource inlineLogo = new LinkedResource(imageUrl, "image/jpeg");
                inlineLogo.ContentId = "hostImage";
                string body = $@"
                <h2><strong>Chúc mừng, bạn có một lịch hẹn mới tại LaLaHome!</strong></h2>
                <p>Khách hàng <strong>{guestName}</strong> đã đặt lịch hẹn xem phòng.</p>
                <p><strong>Số điện thoại:</strong> {SDT}</p>
                <p><strong>Email:</strong> {email}</p>
                <p><strong>Mã phòng:</strong> P{maphong}</p>
                <p><strong>Giá thuê:</strong> {giathue}</p>
                <p><strong>Địa chỉ:</strong> {diachi}, {tenPhuong}, {tenQuan}, {tenTP}</p>
                <p><strong>Ngày xem phòng:</strong> {(ngayxemphong.HasValue ? ngayxemphong.Value.ToString("dd/MM/yyyy HH:mm") : "Không rõ")}</p>
                <p><strong>Ngày dự kiến chuyển vào:</strong> {ngaychuyenvao}</p>
                <p><strong>Số người ở dự kiến:</strong> {songuoio}</p>
                <p><strong>Số lượng xe dự kiến:</strong> {soluongxe}</p>
                <p><strong>Thú cưng:</strong> {thucung}</p>
                <p><strong>Ghi chú của khách:</strong> {ghichu}</p>
                <p>Hãy liên hệ với họ để xác nhận lịch hẹn!</p>
                <p><strong>Chúng tôi rất mong được đón tiếp khách hàng của bạn tại LaLaHome!</strong></p>";

                AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                avHtml.LinkedResources.Add(inlineLogo);
                message.AlternateViews.Add(avHtml);
                smtp.Send(message);
                rs = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                rs = false;
            }
            return rs;
        }









    }

}