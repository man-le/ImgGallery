using ImgStoApi.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgStoApi.Services
{
    interface IImgService
    {
        Task<bool> CreateImg(ImgModel md);
        Task<bool> DeleteImg(string id);
        Task<List<ImgModel>> GetImgs();
        Task<List<ImgModel>> GetUserImgs(string user);
        Task<List<ImgModel>> GetImgByCat(string catName);
        Task<List<CategoryModel>> GetCategories();
        Task<bool> CreateCategory(CategoryModel cat);
        Task<List<ImgModel>> GetUserFav(string userName);
        Task<List<ImgModel>> GetMostViewImg();
        Task<List<ImgModel>> GetPopularImg();
        Task<bool> ConfirmImg(string id);
        Task<bool> CreateSubCat(SubCatUpload sub);
        Task<List<ImgModel>> GetPendingImg();
        Task<bool> DeleteEmptyCat(string catId);
        Task<List<CategoryModel>> GetEmptyCat();
        Task<ImgModel> GetImgById(string id);
        Task<bool> IncreaseView(string imgId);
        Task<bool> AddFavImg(string userId, string imId);
        Task<List<ImgModel>> SearchImg(string content);
        Task<List<ImgModel>> GetSimilar(string imgId);
    }
}
