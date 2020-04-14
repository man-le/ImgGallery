using ImgStoWeb.ApiModels;
using ImgStoWeb.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgStoWeb.ApiSvc
{
    interface IForumSvc
    {
        Task<string> AuthMe(User user);
        Task<bool> ValidateToken(string tok);
        Task<List<ImgModel>> GetUserImgs(string userId);
        Task<User> GetUserByTok(string token);
    }
}
