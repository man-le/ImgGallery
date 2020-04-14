using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ImgStoApi.ApiModels;
using ImgStoApi.BLL;
using ImgStoApi.Utils;

namespace ImgStoApi.Services
{
    public class ImgApiSvc : IImgService
    {
        private ImgBll bll = new ImgBll();

        public async Task<bool> AddFavImg(string userId, string imId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(imId))
                return false;
            return await bll.AddFavImg(userId, imId);
        }

        public async Task<bool> ConfirmImg(string id)
        {
            return await bll.ConfirmImg(id);
        }

        public async Task<bool> CreateCategory(CategoryModel cat)
        {
            if(cat.CategoryId == null || cat.CategoryName == "" || cat.CategoryId==""||cat.CategoryName==null)
            {
                return false;
            }
            return await bll.CreateCategory(cat);
        }

        public async Task<bool> CreateImg(ImgModel md)
        {
            if(md.CategoryName == null || md.CategoryName == ""
                ||md.ImgName == null || md.CategoryName == "")
            {
                return false;
            }
            return await bll.CreateImg(md);
        }

        public async Task<bool> CreateSubCat(SubCatUpload sub)
        {
            if (sub.FatherCat == null || sub.FatherCat == ""
                || sub.Subs == null)
                return false;
            return await bll.CreateSubCat(sub);
        }

        public async Task<bool> DeleteEmptyCat(string catId)
        {
            if(catId == null || catId == "")
            {
                return false;
            }
            return await bll.DeleteCat(catId);
        }

        public async Task<bool> DeleteImg(string id)
        {
            if(id == null || id == "")
            {
                return false;
            }
            return await bll.DeleteImg(id);
        }

        public async Task<List<CategoryModel>> GetCategories()
        {
            return await bll.GetCategories();
        }

        public async Task<List<CategoryModel>> GetEmptyCat()
        {
            return await bll.GetEmptyCat();
        }

        public async Task<List<ImgModel>> GetImgByCat(string catName)
        {
            if(catName == null || catName == "")
            {
                return new List<ImgModel>();
            }
            return await bll.GetImgByCat(catName);
        }

        public async Task<ImgModel> GetImgById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new ImgModel();
            }
            return await bll.GetImgById(id);
        }

        public async Task<List<ImgModel>> GetImgs()
        {
            return await bll.GetImg();
        }

        public async Task<List<ImgModel>> GetMostViewImg()
        {
            var rs = await bll.GetImg();
            int count = 0;
            rs = rs.OrderByDescending(x => x.Views).ToList();
            List<ImgModel> topImgs = new List<ImgModel>();
            for(int i = 0; i < rs.Count(); i++)
            {
                topImgs.Add(rs[i]);
                count += 1;
                if(count == 5)
                {
                    break;
                }
            }
            return topImgs;
        }

        public async Task<List<ImgModel>> GetPendingImg()
        {
            return await bll.GetPendingImgs();
        }

        public async Task<List<ImgModel>> GetPopularImg()
        {
            var rs = await bll.GetImg();
            int count = 0;
            rs = rs.OrderByDescending(x => x.LikeCount).ToList();
            List<ImgModel> topImgs = new List<ImgModel>();
            for (int i = 0; i < rs.Count(); i++)
            {
                topImgs.Add(rs[i]);
                count += 1;
                if (count == 5)
                {
                    break;
                }
            }
            return topImgs;
        }

        public async Task<List<ImgModel>> GetUserFav(string userName)
        {
            if(userName == null || userName == "")
            {
                return null;
            }
            return await bll.GetUserFavoriteImg(userName);
        }

        public async Task<List<ImgModel>> GetUserImgs(string user)
        {
            if(user == null || user == "")
            {
                return null;
            }
            return await bll.GetUserImg(user);
        }

        public async Task<bool> IncreaseView(string imgId)
        {
            if (string.IsNullOrEmpty(imgId))
                return false;
            return await bll.IncreaseView(imgId);
        }

        public async Task<List<ImgModel>> SearchImg(string content)
        {
            if (string.IsNullOrEmpty(content))
                return null;
            if (content == "all")
                return await bll.GetImg();
            if (content.StartsWith("#"))
            {
                return await bll.SearchImgByUser(content.Split(' ')[0]);
            }
            return await bll.SearchImg(content);
        }

        public async Task<List<ImgModel>> GetSimilar(string imgId)
        {
            List<ImgModel> rs = new List<ImgModel>();
            try
            {
                var img = await GetImgById(imgId);
                var ls = await GetImgByCat(img.CategoryName);
                var orHash = GetHash(new Bitmap(PathConfig.IMG_PATH + img.ImgURL.Split('/').Last()));
                for (int i = 0; i < ls.Count; i++)
                {
                    var tempHash = GetHash(new Bitmap(PathConfig.IMG_PATH + ls[i].ImgURL.Split('/').Last()));
                    if (ls[i].ImgId != imgId)
                    {
                        int equalElements = orHash.Zip(tempHash, (k, j) => k == j).Count(eq => eq);
                        if (equalElements == 256)
                        {
                            rs.Add(ls[i]);
                            break;
                        }
                    }
                }

            }catch(Exception ex)
            {

            }
            
            return rs;
        }
        private static List<bool> GetHash(Bitmap bmpSource)
        {
            List<bool> lResult = new List<bool>();
            Bitmap bmpMin = new Bitmap(bmpSource, new Size(16, 16));
            for (int j = 0; j < bmpMin.Height; j++)
            {
                for (int i = 0; i < bmpMin.Width; i++)
                {
                    lResult.Add(bmpMin.GetPixel(i, j).GetBrightness() < 0.5f);
                }
            }
            return lResult;
        }
    }
}