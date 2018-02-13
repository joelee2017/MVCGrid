using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCGrid.Models;
using System.Linq.Dynamic;

namespace MVCGrid.Controllers
{
    public class CategoriesController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: Categories
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult List(int jtStartIndex =0, int jtPageSiez=5, string jtSorting =null)
        {
            string[] OrderByCondition = jtSorting.Split(new char[] { ' ' });//{ ' ' }切開空字元

            string Ordering = string.Format(
                  "{0} {1}", OrderByCondition[0], OrderByCondition[1].Equals("ASC") ? "Ascending" : "Descending");

            IQueryable ResultRecord = db.Categories.OrderBy(Ordering).
                                            Skip(jtStartIndex).Take(jtPageSiez).AsQueryable();

            var result = Json(new { Result = "OK", Records = ResultRecord,
                                                                TotalRecordCount = db.Categories.Count() });

            result.MaxJsonLength = int.MaxValue;

            return result;
        }
    

        //// GET: Categories/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Categories/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryID,CategoryName,Description,Picture")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(categories);
                db.SaveChanges();
                return Json(new { Result = "OK", Record = categories });
            }

            else
            {
                return Json(new { Result = "Error", Message="新增記錄失敗!" });
            }
        }

        //// GET: Categories/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Categories categories = db.Categories.Find(id);
        //    if (categories == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(categories);
        //}

        // POST: Categories/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "CategoryID,CategoryName,Description,Picture")] Categories categories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categories).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Result = "OK", Record = categories });
            }
            else
            {
                return Json(new { Result = "Error", Message = "資料不符合規定!" });
            }
        }

        //// GET: Categories/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Categories categories = db.Categories.Find(id);
        //    if (categories == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(categories);
        //}

        // POST: Categories/Delete/5
        //[HttpPost, ActionName("Delete")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int categoryID)
        {
            Categories categories = db.Categories.Find(categoryID);
            if(categories==null)
            {
                return Json(new { Result = "Error", Message = "找不到欲刪除的記錄!" });
            }
            db.Categories.Remove(categories);
            db.SaveChanges();
            return Json(new { Result = "OK", Record = categories });
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
