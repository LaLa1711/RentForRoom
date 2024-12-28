using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using RentForRoom.DBContext;
using RentForRoom.Models;

namespace RentForRoom.Areas.Admin.Controllers
{
    public class ADatLichesController : Controller
    {
        private QLNhaTroEntities db = new QLNhaTroEntities();

        // GET: Admin/ADatLiches
        public string GetIDPhong(int? id)
        {
            string html = "";

            List<APhongModel> lst = new List<APhongModel>();
            lst = (from grpo in db.tbChiTietPhongs
                   where grpo.Hide != true
                   select (new APhongModel
                   {
                       Id = grpo.IDPhong,
                       Name = grpo.IDPhong,
                       MaTaiKhoan = grpo.MaTaiKhoan
                   })).ToList();
            int tong = lst.Count;

            if (id != null)
            {
                html = "<option value= ''> ----- Chọn -----</option>";
                for (int i = 0; i < tong; i++)
                {
                    if (id == lst[i].Id)
                    {
                        html += "<option selected value='" + lst[i].Id + "'>" + lst[i].MaTaiKhoan + " - ID Phòng" + lst[i].Name + "</option>";
                    }
                    else
                    {
                        html += "<option value='" + lst[i].Id + "'>" + lst[i].MaTaiKhoan + " - ID Phòng" + lst[i].Name + "</option>";

                    }
                }
            }
            else
            {
                html = "<option selected value= ''> ----- Chọn -----</option>";
                for (int i = 0; i < tong; i++)
                {
                    html += "<option value='" + lst[i].Id + "'>" + lst[i].MaTaiKhoan + " - ID Phòng" + lst[i].Name + "</option>";
                }
            }
            return html;

        }
        public ActionResult Index()
        {
            return View(db.tbDatLiches.ToList());
        }
        [HttpPost]
        public JsonResult UpdateStatus(int id, bool hide)
        {
            var user = db.tbDatLiches.Find(id);
            if (user != null)
            {
                user.Hide = hide;
                db.SaveChanges();
            }
            return Json(new { success = true });
        }

        // GET: Admin/ADatLiches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDatLich tbDatLich = db.tbDatLiches.Find(id);
            if (tbDatLich == null)
            {
                return HttpNotFound();
            }
            return View(tbDatLich);
        }

        // GET: Admin/ADatLiches/Create
        public ActionResult Create()
        {
            tbDatLich item = new tbDatLich();
            return View(item);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tbDatLich tbDat)
        {
            tbDatLich item = new tbDatLich();
            try
            {
                item.IDPhong = tbDat.IDPhong;
                item.SoNguoiO = tbDat.SoNguoiO;
                item.SoLuongXe = tbDat.SoLuongXe;
                item.NgayXemPhong = tbDat.NgayXemPhong;
                item.NgayChuyenVao = tbDat.NgayChuyenVao;
                item.GhiChu = tbDat.GhiChu;
                item.ThuCung = tbDat.ThuCung;
                item.HoTen = tbDat.HoTen;
                item.SDT = tbDat.SDT;
                item.Email = tbDat.Email;
                item.Hide = false;
                db.tbDatLiches.Add(item);
                db.SaveChanges();
                return Redirect("/Admin/ADatLiches");
            }
            catch
            {
                return View(tbDat);
            }
        }

        // GET: Admin/ADatLiches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDatLich tbDatLich = db.tbDatLiches.Find(id);
            if (tbDatLich == null)
            {
                return HttpNotFound();
            }
            return View(tbDatLich);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tbDatLich tbDat)
        {
            try
            {
                tbDatLich item = new tbDatLich();

                item = db.tbDatLiches.Find(tbDat.IDDatLich);

                item.IDPhong = tbDat.IDPhong;
                item.SoNguoiO = tbDat.SoNguoiO;
                item.SoLuongXe = tbDat.SoLuongXe;
                item.NgayXemPhong = tbDat.NgayXemPhong;
                item.NgayChuyenVao = tbDat.NgayChuyenVao;
                item.GhiChu = tbDat.GhiChu;
                item.ThuCung = tbDat.ThuCung;
                item.HoTen = tbDat.HoTen;
                item.SDT = tbDat.SDT;
                item.Email = tbDat.Email;
                item.Hide = tbDat.Hide;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/Admin/ADatLiches");
            }
            catch (Exception ex)
            {
                return View(tbDat);
            }
        }

        // GET: Admin/ADatLiches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDatLich tbDatLich = db.tbDatLiches.Find(id);
            if (tbDatLich == null)
            {
                return HttpNotFound();
            }
            return View(tbDatLich);
        }

        // POST: Admin/ADatLiches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbDatLich tbDatLich = db.tbDatLiches.Find(id);
            db.tbDatLiches.Remove(tbDatLich);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
