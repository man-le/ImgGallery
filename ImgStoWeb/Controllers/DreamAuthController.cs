using ImgStoWeb.ApiModels;
using ImgStoWeb.ApiSvc;
using ImgStoWeb.ModelViews;
using ImgStoWeb.UserSvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ImgStoWeb.Controllers
{
    public class DreamAuthController : ApiController
    {
        private IForumSvc svc = new ForumSvc();
        private IApiSvc apiSvc = new ApiSvc.ApiSvc();
        private IUSvc uSvc = new USvc();
        [HttpPost]
        [Route("api/dreamgal/authme")]
        public async Task<JsonResult<string>> Post([FromBody]User value)
        {
            return Json(await svc.AuthMe(value));
        }
        [HttpGet]
        [Route("api/dreamgal/{token}/img")]
        public async Task<JsonResult<List<ImgModel>>> GetAllImg(string token)
        {
            if (! await svc.ValidateToken(token))
            {
                return Json(new List<ImgModel>());
            }
            else
            {
                return Json(await apiSvc.GetImgs());
            }
        }
        [HttpGet]
        [Route("api/dreamgal/{token}/img/search/{content}")]
        public async Task<JsonResult<List<ImgModel>>> SearchImg(string token,string content)
        {
            if(!await svc.ValidateToken(token))
            {
                return Json(new List<ImgModel>());
            }
            else
            {
                return Json(await uSvc.SearchImg(content));
            }
        }
        [HttpGet]
        [Route("api/dreamgal/{token}/user/img")]
        public async Task<JsonResult<List<ImgModel>>> GetUserImg(string token)
        {
            if (!await svc.ValidateToken(token))
                return Json(new List<ImgModel>());
            var userId =await svc.GetUserByTok(token);
            if(string.IsNullOrEmpty(userId.UserId))
                return Json(new List<ImgModel>());
            return Json(await svc.GetUserImgs(userId.UserId));
        }
        [HttpGet]
        [Route("api/dreamgal/{token}/category")]
        public async Task<JsonResult<List<CategoryModel>>> GetCats(string token)
        {
            if (!await svc.ValidateToken(token))
                return Json(new List<CategoryModel>());
            return Json(await apiSvc.GetCategories());
        }
        [HttpGet]
        [Route("api/dreamgal/{token}/{category}/img")]
        public async Task<JsonResult<List<ImgModel>>> GetImgByCat(string token, string category)
        {
            if (!await svc.ValidateToken(token))
                return Json(new List<ImgModel>());
            return Json(await uSvc.GetImgByCat(category));
        }

    }
}