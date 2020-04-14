using ImgStoWeb.ApiModels;
using ImgStoWeb.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ImgStoWeb.ApiSvc
{
    public interface IApiSvc
    {
        Task<List<CategoryModel>> GetCategories();
        Task<bool> PostImg(ImgModel model);
        Task<bool> PostCategory(CategoryModel category);
        Task<bool> PostSubCategory(SubCatUpload sub);
        Task<bool> DeleteCategory(string catId);
        Task<bool> UploadImg(HttpPostedFileBase file, string fileName);
        Task<List<ImgModel>> GetImgs();
        Task<ImgModel> GetImgById(string id);
        Task<List<ImgModel>> GetPendingImg();
        Task<bool> ConfirmImg(string id);
        Task<List<ImgModel>> GetDups(string id);
        Task<bool> DeleteImg(string id);
    }
}