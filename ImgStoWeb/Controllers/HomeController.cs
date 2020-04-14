using ImgStoWeb.ApiModels;
using ImgStoWeb.ApiSvc;
using ImgStoWeb.ModelViews;
using ImgStoWeb.Services;
using ImgStoWeb.UserSvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ImgStoWeb.Controllers
{
    public class HomeController : Controller
    {
        private IGalSvc galSvc = new GalSvc();
        protected override  void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            if (String.IsNullOrEmpty((string)Session["User"]))
            {
                if (Request.Cookies["DreamGalCook"] != null)
                {
                    MyCookie cookie = JsonConvert.DeserializeObject<MyCookie>(Request.Cookies["DreamGalCook"].Value);
                    var rs = galSvc.ConfirmToken(cookie.UserId, cookie.Token);
                    
                    //Session["User"] = cookie.UserId;
                    if (rs)
                        Session["User"] = cookie.UserId;
                    else
                    {
                        Request.Cookies["DreamGalCook"].Expires = DateTime.Now.AddDays(-1);
                        Request.Cookies.Add(Request.Cookies["DreamGalCook"]);
                    }
                }
            }
        }
        private IUSvc svc = new UserSvc.USvc();
        [ChildActionOnly]
        public async Task<ActionResult> SideBar()
        {
            var cats = await svc.GetCategories();
            return PartialView("_HomeSidebar", cats);
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> Category(string subCat)
        {
            ViewBag.Imgs = await svc.GetImgByCat(subCat);
            return View();
        }
        [HttpPost]
        public async Task<HttpStatusCode> HandleViewCount()
        {
            string imgId = Request["imgId"];
            if(await svc.IncreaseViews(imgId.ToString()))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }
        public ActionResult Login()
        {
            if (Session["User"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();

        }
        public ActionResult SignUp()
        {
            if (Session["User"] != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<ActionResult> MyFavorite()
        {
            ViewBag.Imgs = await svc.GetFavImg((string)Session["User"]);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> HandleSignUp(User user)
        {
            try
            {
                user.UserId = user.UserId.Trim();
                user.UserId = user.UserId.ToLower();
                if(await galSvc.SignUp(user))
                {
                    return RedirectToAction("Login");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("SignUp");
            }
            return RedirectToAction("SignUp");

        }
        [HttpPost]
        public async Task<ActionResult> HandleSignIn(User user)
        {
            try
            {
                user.UserId = user.UserId.Trim();
                user.UserId = user.UserId.ToLower();
                if (await galSvc.SignIn(user))
                {
                    if (ModelState.IsValid)
                    {
                        var token = await galSvc.GetUserToken(user.UserId);
                        MyCookie cook = new MyCookie()
                        {
                            Token = token,
                            UserId = user.UserId
                        };
                        var json = JsonConvert.SerializeObject(cook);
                        HttpCookie userCookie = new HttpCookie("DreamGalCook");
                        userCookie.Value = json;
                        userCookie.Expires.AddYears(1);
                        HttpContext.Response.Cookies.Add(userCookie);
                        RedirectToAction("Index");
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public async Task<HttpStatusCode> HandleFav()
        {
            string imgId = Request["imgId"];
            if (await svc.AddToFav((string)Session["User"],imgId.ToString()))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }
        public async Task<ActionResult> MostView()
        {
            ViewBag.Imgs =await svc.GetMostView();
            return View();
        }
        public async Task<ActionResult> Popular()
        {
            ViewBag.Imgs =await svc.GetPopular();
            return View();
        }
        public async Task<ActionResult> Upload()
        {
            IApiSvc apiSvc = new ApiSvc.ApiSvc();
            var cat = await apiSvc.GetCategories();
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
        [HttpPost]
        public async Task<ActionResult> HandleCreateImg(HttpPostedFileBase postedFile)
        {
            try
            {
                IApiSvc apiSvc = new ApiSvc.ApiSvc();
                ImgModel model = new ImgModel()
                {
                    ImgName = Request.Params[0],
                    CategoryName = Request.Params[1],
                    UploadBy = Session["User"].ToString()
                };

                model.ImgURL = "IMG" + "_" + DateTime.Now.Ticks + Path.GetExtension(postedFile.FileName);
                var flag = await apiSvc.PostImg(model);
                if (flag)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        HttpPostedFileBase file = Request.Files[i];
                        await apiSvc.UploadImg(file, model.ImgURL.Split('.')[0]);
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
            return RedirectToAction("Upload");
        }
        [HttpPost]
        public async Task<JsonResult> HandleSearch()
        {
            string content = Request.Params["content"];
            return Json(await svc.SearchImg(content));
        }
        public async Task<ActionResult> UserInfo()
        {
            var user =await galSvc.GetUser((string)Session["User"]);
            return View(user);
        }
        [HttpPost]
        public HttpStatusCode HandleSignOut()
        {
            if (Request.Cookies["DreamGalCook"] != null)
            {
                var c = new HttpCookie("DreamGalCook");
                Session["User"] = null;
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            return HttpStatusCode.OK;
        }
        [HttpPost]
        public async Task<ActionResult> HandleChangePwd(User user)
        {
            if (await galSvc.ChangePwd(user))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("UserInfo");
        }
    }
}