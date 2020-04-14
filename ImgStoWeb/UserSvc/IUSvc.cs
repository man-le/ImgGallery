using ImgStoWeb.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgStoWeb.UserSvc
{
    interface IUSvc
    {
        Task<List<CategoryModel>> GetCategories();
        Task<List<ImgModel>> GetImgByCat(string catId);
        Task<bool> IncreaseViews(string imgId);
        Task<List<ImgModel>> GetFavImg(string userId);
        Task<bool> AddToFav(string userId, string imgId);
        Task<List<ImgModel>> GetMostView();
        Task<List<ImgModel>> GetPopular();
        Task<List<ImgModel>> SearchImg(string content);
    }
}
