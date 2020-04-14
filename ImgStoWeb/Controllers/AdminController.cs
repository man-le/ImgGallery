using ImgStoWeb.ApiModels;
using ImgStoWeb.ApiSvc;
using ImgStoWeb.ModelViews;
using ImgStoWeb.Services;
using ImgStoWeb.Utils;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ImgStoWeb.Controllers
{
    public class AdminController : Controller
    {
        private IApiSvc svc = new ApiSvc.ApiSvc();
        private IAdminSvc adSvc = new AdminSvc();
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (Session["Admin"] == null)
            {
                requestContext.HttpContext.Response.Clear();
                requestContext.HttpContext.Response.Redirect((Url.Action("Login", "AdminLogin")));
                requestContext.HttpContext.Response.End();
            }

        }
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            ViewBag.UserCount = await adSvc.GetNumberOfUser();
            var cats = await svc.GetCategories();
            ViewBag.CatCount = cats.Count();
            int subCount = 0;
            foreach (var item in cats)
            {
                subCount += item.SubCategoryName.Count();
            }
            ViewBag.SubCount = subCount;
            var imgs = await svc.GetImgs();
            ViewBag.ImgCount = imgs.Count();
            var pendings = await svc.GetPendingImg();
            ViewBag.PendingCount = pendings.Count();
            return View();
        }
        public async Task<ActionResult> ListImgs(int? pageNumber)
        {
            var ls = await svc.GetImgs();
            if (ls is null)
                ls = new List<ImgModel>();
            return View(ls.ToPagedList(pageNumber ?? 1, 10));
        }
        public async Task<ActionResult> CreateImgs()
        {
            var cat = await svc.GetCategories();
            List<string> subCat = new List<string>();
            foreach (var item in cat)
            {
                foreach (var sub in item.SubCategoryName)
                {
                    subCat.Add(sub);
                }
            }
            ViewBag.Categories = subCat;
            return View();
        }
        public async Task<ActionResult> ImgDet(string imgId)
        {
            return View(await svc.GetImgById(imgId));
        }
        public ActionResult CreateCategory()
        {
            return View();
        }
        public async Task<ActionResult> PendingImgs()
        {
            var imgs = await svc.GetPendingImg();
            List<string> dup = new List<string>();
            foreach (var item in imgs)
            {
                var temp = await svc.GetDups(item.ImgId);
                if (temp.Count != 0)
                    dup.Add("Duplicated with " + temp.First().ImgName + $"</br> <a href=\"{temp.First().ImgURL}\">Detail</a>");
                else
                {
                    dup.Add("");
                }
            }
            ViewBag.Imgs = imgs;
            ViewBag.IsDup = dup;
            return View();
        }
        public async Task<ActionResult> CreateSubCat()
        {
            ViewBag.Category = await svc.GetCategories();
            return View();
        }
        public async Task<ActionResult> ListCategories(int? pageNumber)
        {
            var ls = await svc.GetCategories();
            return View(ls.ToPagedList(pageNumber ?? 1, 10));
        }
        public async Task<ActionResult> ListSub(int? pageNumber)
        {
            var ls =await svc.GetCategories();
            return View(ls.ToPagedList(pageNumber ?? 1, 10));
        }
        public async Task<ActionResult> ConfirmImg(string imgId)
        {
            try
            {
                if (await svc.ConfirmImg(imgId))
                    TempData["IsSuccess"] = true;
                else
                    TempData["IsSuccess"] = false;
            }
            catch
            {
                TempData["IsSuccess"] = false;
            }
            return RedirectToAction("PendingImgs");
        }
        public async Task<ActionResult> DeleteImg(string imgId)
        {
            var id = imgId;
            try
            {

                if (await svc.DeleteImg(id))
                    TempData["IsSuccess"] = true;
                else
                    TempData["IsSuccess"] = false;
            }
            catch
            {
                TempData["IsSuccess"] = false;
            }
            return RedirectToAction("PendingImgs");
        }
        /////////////////////////Handle//////////////////////////
        [HttpPost]
        public async Task<ActionResult> HandleCreateImg(HttpPostedFileBase postedFile)
        {
            try
            {
                ImgModel model = new ImgModel()
                {
                    ImgName = Request.Params[0],
                    CategoryName = Request.Params[1],
                    UploadBy = Session["Admin"].ToString()
                };

                model.ImgURL = "IMG" + "_" + DateTime.Now.Ticks + Path.GetExtension(postedFile.FileName);
                var flag = await svc.PostImg(model);
                if (flag)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        await svc.UploadImg(file, model.ImgURL.Split('.')[0]);
                    }
                }
                else
                {
                    TempData["IsSuccess"] = false;
                }
            }
            catch
            {
                TempData["IsSuccess"] = false;
            }
            return RedirectToAction("CreateImgs");
        }
        [HttpPost]
        public async Task<ActionResult> HandleCreateCat(CategoryModel category)
        {
            try
            {
                if (await svc.PostCategory(category))
                {
                    TempData["IsSuccess"] = true;
                }
                else
                {
                    TempData["IsSuccess"] = false;
                }
            }
            catch
            {
                TempData["IsSuccess"] = false;
                throw;
            }
            return RedirectToAction("CreateCategory");
        }
        public async Task<ActionResult> HandleSubCat(SubCat sub)
        {
            try
            {
                List<SubCatModel> cats = new List<SubCatModel>();
                cats.Add(new SubCatModel()
                {
                    CatId = sub.SubId,
                    CatName = sub.SubName
                });
                SubCatUpload subCat = new SubCatUpload()
                {
                    FatherCat = sub.FatherName,
                    Subs = cats
                };
                if (await svc.PostSubCategory(subCat))
                {
                    TempData["IsSuccess"] = true;

                }
                else
                {
                    TempData["IsSuccess"] = false;

                }
            }
            catch
            {
                TempData["IsSuccess"] = false;
            }
            return RedirectToAction("CreateSubCat");
        }
        [HttpGet]
        public async Task<JsonResult> HandleAreaData()
        {
            var ls = adSvc.GetChartData(await svc.GetImgs());
            return Json(ls, JsonRequestBehavior.AllowGet);
        }
    }
}