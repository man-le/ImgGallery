using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Routing;

namespace ImgStoApi.Utils
{
    public class PathConfig
    {
        public static string IMG_URL = "http://dreamapi.tk/content/uploads/imgs/";
        public static string IMG_PATH = HttpContext.Current.Server.MapPath("~/Content/uploads/imgs/");
    }
}