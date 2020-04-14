using DoctorForum.Utils;
using ImgStoApi.ApiModels;
using ImgStoApi.Models;
using ImgStoApi.Repositories;
using ImgStoApi.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MongoDB.Bson.Serialization.BsonDeserializationContext;

namespace ImgStoApi.BLL
{
    public class ImgBll
    {
        private UnitOfWork uow = new UnitOfWork();
        private async Task<tblCategory> GetCat(string catName)
        {
            try
            {
                var repo = uow.CategoryRepo();
                foreach (var item in await repo.GetEntities())
                {
                    if (item.CatName == catName)
                    {
                        return item;
                    }
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        private async Task<int> GetLikes(string imgId)
        {
            try
            {
                int count = 0;
                var repo = uow.ImgRatingRepo();
                foreach (var item in await repo.GetEntities())
                {
                    if (item.ImgId == imgId)
                    {
                        if (item.IsLike)
                        {
                            count += 1;
                        }
                    }
                }
                return count;
            }
            catch
            {
                return 0;
            }
        }
        public async Task<List<ImgModel>> GetImg()
        {
            try
            {
                var repo = uow.ImageRepo();
                var rs = await repo.GetEntities();
                List<ImgModel> ls = new List<ImgModel>();
                foreach (var item in rs)
                {
                    if (item.IsValid != false)
                    {
                        ls.Add(new ImgModel
                        {
                            CategoryName = item.Cat.CatName,
                            UploadDate = TimeStamp.GetDateFromUnix(long.Parse(item.UploadedDate)).ToString(),
                            ImgName = item.ImgName,
                            ImgURL = PathConfig.IMG_URL + item.FileName,
                            LikeCount = await GetLikes(item.ImgId),
                            UploadBy = item.UploadedBy ?? "",
                            Views = item.View,
                            ImgId = item.ImgId
                        });
                    }
                }
                return ls;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> CreateImg(ImgModel img)
        {
            try
            {
                var repo = uow.ImageRepo();
                List<tblImg> ls = new List<tblImg>();
                ls.Add(new tblImg()
                {
                    Cat = await GetCat(img.CategoryName),
                    ImgId = "IMG" + DateTime.Now.Ticks,
                    ImgName = img.ImgName,
                    UploadedDate = TimeStamp.GetTimeNowInUnix().ToString(),
                    UploadedBy = img.UploadBy,
                    FileName = img.ImgURL,
                    View = 1,
                    IsValid = false
                });
                return await repo.AddEntities(ls);
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<ImgModel>> GetUserImg(string userName)
        {
            try
            {
                var repo = uow.ImageRepo();
                List<ImgModel> ls = new List<ImgModel>();
                foreach (var item in await repo.GetEntities())
                {
                    if (item.UploadedBy == userName)
                    {
                        ls.Add(new ImgModel()
                        {
                            ImgId = item.ImgId,
                            CategoryName = item.Cat.CatName,
                            ImgURL = PathConfig.IMG_URL + item.FileName,
                            LikeCount = await GetLikes(item.ImgId),
                            UploadDate = TimeStamp.GetDateFromUnix(long.Parse(item.UploadedDate)).ToString(),
                            UploadBy = item.UploadedBy,
                            Views = item.View,
                            ImgName = item.ImgName
                        });
                    }
                }
                return ls;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<ImgModel>> GetUserFavoriteImg(string userName)
        {
            try
            {
                List<ImgModel> ls = new List<ImgModel>();
                var imgRepo = uow.ImageRepo();
                var rateRepo = uow.ImgRatingRepo();
                var lsImgs = await imgRepo.GetEntities();
                var rateImgs = await rateRepo.GetEntities();
                var query = from i in lsImgs
                            join r in rateImgs
                            on i.ImgId equals r.ImgId
                            select new
                            {
                                i.ImgName,
                                i.UploadedDate,
                                i.UploadedBy,
                                r.IsLike,
                                i.ImgId,
                                i.View,
                                i.Cat,
                                i.FileName
                            };
                foreach (var item in query)
                {
                    if (item.IsLike == true)
                    {
                        ls.Add(new ImgModel()
                        {
                            UploadDate = TimeStamp.GetDateFromUnix(long.Parse(item.UploadedDate)).ToString(),
                            UploadBy = item.UploadedBy,
                            CategoryName = item.Cat.CatName,
                            ImgName = item.ImgName,
                            ImgURL = PathConfig.IMG_URL + item.FileName,
                            LikeCount = await GetLikes(item.ImgId),
                            Views = item.View,
                            ImgId = item.ImgId
                        });
                    }
                }
                return ls;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<CategoryModel>> GetCategories()
        {
            try
            {
                List<CategoryModel> ls = new List<CategoryModel>();
                var repo = uow.CategoryRepo();
                foreach (var item in await repo.GetEntities())
                {
                  
                    List<string> subCat = new List<string>();
                    if (item.SubCat != null)
                    {
                        var temp = new CategoryModel()
                        {
                            CategoryId = item.CatId,
                            CategoryName = item.CatName
                        };
                        foreach (var cat in item.SubCat)
                        {
                            subCat.Add(cat.CatName);
                        }
                        temp.SubCategoryName = subCat;
                        ls.Add(temp);
                    }
                   
                }
                return ls;
            }
            catch
            {
                return null;
            }
        }
        public async Task<List<CategoryModel>> GetEmptyCat()
        {
            try
            {
                var repo = uow.CategoryRepo();
                List<CategoryModel> ls = new List<CategoryModel>();
                foreach(var item in await repo.GetEntities())
                {
                    if(item.SubCat != null)
                    {
                        if(item.SubCat.Count == 0)
                        {
                            ls.Add(new CategoryModel()
                            {
                                CategoryId = item.CatId,
                                CategoryName = item.CatName
                            });
                        }
                    }
                }
                return ls;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> DeleteCat(string catId)
        {
            try
            {
                var repo = uow.CategoryRepo();
                bool flag = false;
                foreach (var item in await repo.GetEntities())
                {
                    if (item.SubCat != null)
                    {
                        if (item.SubCat.Count == 0 && item.CatId == catId)
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    var filter = Builders<tblCategory>.Filter.Eq(x => x.CatId, catId);
                    return await repo.DeleteEntities(filter);
                }
                return flag;
            }
            catch 
            {
                return false;
                throw;
            }
        }
        public async Task<List<ImgModel>> GetImgByCat(string catName)
        {
            try
            {
                var imgRepo = uow.ImageRepo();
                List<ImgModel> ls = new List<ImgModel>();
                foreach (var img in await imgRepo.GetEntities())
                {
                    if (img.Cat.CatName == catName && img.IsValid)
                    {
                        ls.Add(new ImgModel()
                        {
                            CategoryName = img.Cat.CatName,
                            ImgName = img.ImgName,
                            ImgURL = PathConfig.IMG_URL + img.FileName,
                            LikeCount = await GetLikes(img.ImgId),
                            UploadBy = img.UploadedBy??"",
                            UploadDate = TimeStamp.GetDateFromUnix(long.Parse(img.UploadedDate)).ToString(),
                            Views = img.View,
                            ImgId = img.ImgId
                        });
                    }
                }
                return ls;
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> DeleteImg(string imgId)
        {
            try
            {
                var repo = uow.ImageRepo();
                var filter = Builders<tblImg>.Filter.Eq(x => x.ImgId, imgId);
                return await repo.DeleteEntities(filter);
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> ConfirmImg(string imgId)
        {
            try
            {
                var repo = uow.ImageRepo();
                tblImg temp = new tblImg();
                foreach (var item in await repo.GetEntities())
                {
                    if (item.ImgId == imgId)
                    {
                        temp = item;
                        break;
                    }
                }
                if (temp != null)
                {
                    temp.IsValid = true;
                    var filter = Builders<tblImg>.Filter.Eq(x => x.ImgId, imgId);
                    return await repo.UpdateEntities(filter, temp);
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<ImgModel>> GetPendingImgs()
        {
            try
            {
                var repo = uow.ImageRepo();
                List<ImgModel> ls = new List<ImgModel>();
                foreach(var item in await repo.GetEntities())
                {
                    if (!item.IsValid)
                    {
                        ls.Add(new ImgModel()
                        {
                            CategoryName = item.Cat.CatName,
                            ImgId = item.ImgId,
                            ImgURL = PathConfig.IMG_URL + item.FileName,
                            ImgName = item.ImgName,
                            UploadBy = item.UploadedBy ?? "",
                            UploadDate = TimeStamp.GetDateFromUnix(long.Parse(item.UploadedDate)).ToString()
                        });
                    }
                }
                return ls;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> CreateCategory(CategoryModel model)
        {
            try
            {
                var repo = uow.CategoryRepo();
                List<tblCategory> ls = new List<tblCategory>();
                ls.Add(new tblCategory
                {
                    CatId = model.CategoryId,
                    CatName = model.CategoryName,
                    SubCat = new List<tblCategory>()
                });
                return await repo.AddEntities(ls);
            }
            catch
            {
                return false;
            }
        }
        private async Task<tblCategory> IsCatExits(string catId)
        {
            try
            {
                var repo = uow.CategoryRepo();
                var filter = Builders<tblCategory>.Filter.Eq(x => x.CatId, catId);
                var rs = await repo.CustomQuery(filter);
                if (rs.Count() != 0)
                    return rs.First();
                return null;
            }
            catch
            {
                throw new Exception("Error occurs");
            }
        }
        public async Task<bool> CreateSubCat(SubCatUpload subs)
        {
            try
            {
                var repo = uow.CategoryRepo();
                var id = await GetCat(subs.FatherCat);
                var temp = await IsCatExits(id.CatId);
              
                if (null != temp)
                {
                    
                    foreach (var item in subs.Subs)
                    {
                        if (temp.SubCat.Count() == 0)
                        {
                            var cat = new tblCategory
                            {
                                CatId = item.CatId,
                                CatName = item.CatName,
                                SubCat = null
                            };
                            temp.SubCat.Add(cat);
                            await CreateSubCat(cat);
                        }
                        else
                        {
                            int length = temp.SubCat.Count();
                            for(int i = 0; i < length;i++)
                            {
                                if (item.CatId != temp.SubCat[i].CatId && item.CatName != temp.SubCat[i].CatName)
                                {
                                    var cat = new tblCategory
                                    {
                                        CatId = item.CatId,
                                        CatName = item.CatName,
                                        SubCat = null
                                    };
                                    temp.SubCat.Add(cat);
                                    await CreateSubCat(cat);
                                }
                            }
                        }
                    }
                    return await repo.UpdateEntities(Builders<tblCategory>.Filter.Eq(x => x.CatId, temp.CatId), temp);
                }
                return false;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> CreateSubCat(tblCategory cat)
        {
            try
            {
                var repo = uow.CategoryRepo();
                List<tblCategory> ls = new List<tblCategory>();
                ls.Add(cat);
                return await repo.AddEntities(ls);
            }
            catch
            {
                return false;
            }
        }
        public async Task<ImgModel> GetImgById(string id)
        {
            var repo = uow.ImageRepo();
            try
            {
                var filter = Builders<tblImg>.Filter.Eq(x => x.ImgId, id);
                var rs = await repo.CustomQuery(filter);
                return new ImgModel()
                {
                    CategoryName = rs.FirstOrDefault().Cat.CatName,
                    ImgId = rs.FirstOrDefault().ImgId,
                    ImgName = rs.FirstOrDefault().ImgName,
                    ImgURL = PathConfig.IMG_URL + rs.FirstOrDefault().FileName,
                    UploadBy = rs.FirstOrDefault().UploadedBy,
                    UploadDate = TimeStamp.GetDateFromUnix(long.Parse(rs.FirstOrDefault().UploadedDate)).ToString(),
                    LikeCount = await GetLikes(rs.FirstOrDefault().ImgId),
                    Views = rs.FirstOrDefault().View
                };
            }
            catch
            {
                return new ImgModel();
            }
        }
        public async Task<bool> IncreaseView(string imgId)
        {
            try
            {
                var repo = uow.ImageRepo();
                tblImg tbl = new tblImg();
                foreach(var item in await repo.GetEntities())
                {
                    if (item.ImgId == imgId)
                        tbl = item;
                }
                tbl.View += 1;
                var filter = Builders<tblImg>.Filter.Eq(x => x.ImgId, imgId);
                return await repo.UpdateEntities(filter, tbl);
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> AddFavImg(string userId, string imgId)
        {
            try
            {
                var repo = uow.ImgRatingRepo();
                foreach(var item in await repo.GetEntities())
                {
                    if (item.UserName == userId && item.ImgId == imgId)
                        return true;
                }
                tblImgRating img = new tblImgRating()
                {
                    ImgId = imgId,
                    IsLike = true,
                    UserName = userId
                };
                List<tblImgRating> ls = new List<tblImgRating>();
                ls.Add(img);
                return await repo.AddEntities(ls);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<List<ImgModel>> SearchImg(string content)
        {
            try
            {
                var repo = uow.ImageRepo();
                List<ImgModel> models = new List<ImgModel>();
                foreach(var item in await repo.GetEntities())
                {
                    if(item.ImgName.ToLower().StartsWith(content) || item.ImgName.ToLower().Contains(content))
                    {
                        models.Add(new ImgModel()
                        {
                            CategoryName = item.Cat.CatName,
                            ImgId = item.ImgId,
                            ImgName = item.ImgName,
                            LikeCount = await GetLikes(item.ImgId),
                            ImgURL = PathConfig.IMG_URL + item.FileName,
                            UploadDate = TimeStamp.GetDateFromUnix(long.Parse(item.UploadedDate)).ToString(),
                            UploadBy = item.UploadedBy
                        });
                    }
                }
                return models;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<ImgModel>> SearchImgByUser(string userId)
        {
            List<ImgModel> ls = new List<ImgModel>();
            try
            {
                var repo = uow.ImageRepo();
                foreach (var item in await repo.GetEntities())
                {
                    if (item.UploadedBy == userId.Split('#')[1])
                    {
                        ls.Add(new ImgModel()
                        {
                            CategoryName = item.Cat.CatName,
                            ImgId = item.ImgId,
                            ImgName = item.ImgName,
                            ImgURL = PathConfig.IMG_URL + item.FileName,
                            UploadDate = TimeStamp.GetDateFromUnix(long.Parse(item.UploadedDate)).ToString(),
                            LikeCount = await GetLikes(item.ImgId),
                            UploadBy = item.UploadedBy,
                            Views = item.View
                        });
                    }
                }
                return ls;
            }
            catch (Exception)
            {
                return ls;
            }
        }
    }
}