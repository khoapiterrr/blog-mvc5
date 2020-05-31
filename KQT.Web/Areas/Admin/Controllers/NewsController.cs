﻿using KQT.DataMigration;
using KQT.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KQT.Web.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private DataMigrationContext db = new DataMigrationContext();

        #region News

        public ActionResult Index()
        {
            List<NewsEntity> news = db.NewsEntities.ToList();

            return View(news);
        }

        public ActionResult CreateNews()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNews(NewsEntity news, HttpPostedFileBase fUploadHead, HttpPostedFileBase fUploadBody, HttpPostedFileBase fUploadFooter)
        {
            if (ModelState.IsValid)
            {
                if (CheckFile(fUploadHead, "News"))
                {
                    news.ImageHead = fUploadHead.FileName;
                }
                if (CheckFile(fUploadBody, "News"))
                {
                    news.ImageBody = fUploadBody.FileName;
                }
                if (CheckFile(fUploadFooter, "News"))
                {
                    news.ImageFooter = fUploadFooter.FileName;
                }
                news.CreatedDate = DateTime.Now;
                news.Id = Guid.NewGuid().ToString();
                db.NewsEntities.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return null;
        }

        [HttpGet]
        public ActionResult EditNews(string id)
        {
            NewsEntity news = db.NewsEntities.FirstOrDefault(x => x.Id == id);
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult EditNews(NewsEntity news, string id, HttpPostedFileBase fUploadHead, HttpPostedFileBase fUploadBody, HttpPostedFileBase fUploadFooter)
        {
            if (ModelState.IsValid)
            {
                if (CheckFile(fUploadHead, "News"))
                {
                    news.ImageHead = fUploadHead.FileName;
                }
                if (CheckFile(fUploadBody, "News"))
                {
                    news.ImageBody = fUploadBody.FileName;
                }
                if (CheckFile(fUploadFooter, "News"))
                {
                    news.ImageFooter = fUploadFooter.FileName;
                }
                news.UpdatedDate = DateTime.Now;
                NewsEntity oldNews = db.NewsEntities.FirstOrDefault(x => x.Id == id);
                db.Entry(oldNews).CurrentValues.SetValues(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return null;
        }

        [HttpGet]
        public ActionResult DetailsNews(string id)
        {
            NewsEntity news = db.NewsEntities.FirstOrDefault(x => x.Id == id);
            return View(news);
        }

        public JsonResult DeleteNews(string id)
        {
            try
            {
                var data = db.NewsEntities.FirstOrDefault(x => x.Id == id);
                if (data == null)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Không tìm thấy đối tượng cần xóa."
                    }, JsonRequestBehavior.AllowGet);
                }
                db.NewsEntities.Remove(data);
                db.SaveChanges();
                return Json(new
                {
                    Success = true,
                    Message = "Xóa thành công kích thước này."
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Success = false,
                    Message = "Không thể xóa đối tượng này. Vì sẽ ảnh hưởng đến dữ liệu khác." + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion News

        private Boolean CheckFile(HttpPostedFileBase fUpload, string path)
        {
            if (fUpload != null && fUpload.ContentLength > 0)
            {
                fUpload.SaveAs(Server.MapPath("~/Content/img/" + path + "/ " + fUpload.FileName));
                return true;
            }
            return false;
        }
    }
}