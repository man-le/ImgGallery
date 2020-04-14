using ImgStoApi.ApiModels;
using ImgStoApi.Services;
using ImgStoApi.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ImgStoApi.Controllers
{
    public class ImgController : ApiController
    {
        private IImgService services = new ImgApiSvc();
        [HttpPost]
        [Route("api/img/upload/{file_name}")]
        public async Task<HttpStatusCode> UploadImg(string file_name)
        {
            try
            {
                ///-----Check auth here------
                ///
                /// 
                /// 
                /// 
                ///------End check-------
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable,
                                                                           "This request is not properly formatted - not multipart."));
                }

                var provider = new UploadFilePro();

                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (HttpContent ctnt in provider.Contents)
                {
                    var stream = await ctnt.ReadAsStreamAsync();

                    if (stream.Length != 0)
                    {
                        using (FileStream file = new FileStream(PathConfig.IMG_PATH + file_name + "." + provider.ext, FileMode.Create, FileAccess.Write))
                            stream.CopyTo(file);
                    }
                }
                return HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return HttpStatusCode.BadRequest;
            }
        }
        [HttpGet]
        [Route("api/img")]
        public async Task<JsonResult<List<ImgModel>>> GetImg()
        {
            return Json(await services.GetImgs());
        }
        [HttpGet]
        [Route("api/img/{img_id}")]
        public async Task<JsonResult<ImgModel>> GetImgById(string img_id)
        {
            return Json(await services.GetImgById(img_id));
        }
        [HttpGet]
        [Route("api/category/{categoryName}/img")]
        public async Task<JsonResult<List<ImgModel>>> GetImgCat(string categoryName)
        {
            return Json(await services.GetImgByCat(categoryName));
        }
        [HttpPost]
        [Route("api/img")]
        public async Task<HttpStatusCode> PostImg([FromBody]ImgModel value)
        {
            if (await services.CreateImg(value))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }
        [HttpDelete]
        [Route("api/img")]
        public async Task<HttpStatusCode> DeleteImg(string id)
        {
            if(await services.DeleteImg(id))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }
        [HttpGet]
        [Route("api/category")]
        public async Task<JsonResult<List<CategoryModel>>> GetCat()
        {
            return Json(await services.GetCategories());
        }
        [HttpPost]
        [Route("api/category")]
        public async Task<HttpStatusCode> PostCat([FromBody]CategoryModel mod)
        {
            if(await services.CreateCategory(mod)){
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;

        }
        [HttpPost]
        [Route("api/category/sub")]
        public async Task<HttpStatusCode> PostSub([FromBody]SubCatUpload mod)
        {
            if(await services.CreateSubCat(mod))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }
        [HttpGet]
        [Route("api/img/mostview")]
        public async Task<JsonResult<List<ImgModel>>> GetMostView()
        {
            return Json(await services.GetMostViewImg());
        }
        [HttpGet]
        [Route("api/img/popular")]
        public async Task<JsonResult<List<ImgModel>>> GetPop()
        {
            return Json(await services.GetPopularImg());
        }
        [HttpGet]
        [Route("api/user/{user_id}/img")]
        public async Task<JsonResult<List<ImgModel>>> GetUserImg(string user_id)
        {
            return Json(await services.GetUserImgs(user_id));
        }
        [HttpGet]
        [Route("api/user/{user_id}/img/fav")]
        public async Task<JsonResult<List<ImgModel>>> GetUserFav(string user_id)
        {
            return Json(await services.GetUserFav(user_id));
        }
        [HttpGet]
        [Route("api/admin/pending/img")]
        public async Task<JsonResult<List<ImgModel>>> GetPending(){
            return Json(await services.GetPendingImg());
        }
        [HttpPost]
        [Route("api/admin/img/{img_id}/confirm")]
        public async Task<HttpStatusCode> ConfirmImg(string img_id)
        {
            if(await services.ConfirmImg(img_id))
            {
                return HttpStatusCode.OK;
            }
            return HttpStatusCode.BadRequest;
        }
        [HttpGet]
        [Route("api/admin/category/empty")]
        public async Task<JsonResult<List<CategoryModel>>> GetEmptyCat()
        {
            return Json(await services.GetEmptyCat());
        }
        [HttpDelete]
        [Route("api/admin/category/{cat_id}/empty")]
        public async Task<bool> DeleteEmptyCat(string cat_id)
        {
            if(await services.DeleteEmptyCat(cat_id))
            {
                return true;
            }
            return false;
        }
        [HttpPost]
        [Route("api/img/{imgId}/view")]
        public async Task<HttpStatusCode> IncreaseView(string imgId)
        {
            if (await services.IncreaseView(imgId))
                return HttpStatusCode.OK;
            return HttpStatusCode.BadRequest;
        }
        [HttpPost]
        [Route("api/user/{user_id}/img/{img_id}/fav")]
        public async Task<HttpStatusCode> AddFavImg(string user_id,string img_id)
        {
            if (await services.AddFavImg(user_id, img_id))
                return HttpStatusCode.OK;
            return HttpStatusCode.BadRequest;
        }
        [HttpGet]
        [Route("api/img/search/{content}")]
        public async Task<JsonResult<List<ImgModel>>> SearchImg(string content)
        {
            return Json(await services.SearchImg(content.ToLower()));
        }
        [HttpGet]
        [Route("api/img/similar/{imgId}")]
        public async Task<JsonResult<List<ImgModel>>> GetSimiImg(string imgId)
        {
            return Json(await services.GetSimilar(imgId));
        }
    }
}