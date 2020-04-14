using ImgStoWeb.ModelViews;
using ImgStoWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ImgStoWeb.Controllers
{
    public class AdminLoginController : Controller
    {
        public ActionResult Login()
        {
            return View("~/Views/Admin/Login.cshtml");
        }
        [HttpPost]
        public async Task<ActionResult> HandleLogin(User user)
        {
            IGalSvc galSvc = new GalSvc();
            if (await galSvc.AdminLogin(user))
            {
                Session["Admin"] = user.UserId;
            }
            return RedirectToAction("Index","Admin");
        }
    }
}