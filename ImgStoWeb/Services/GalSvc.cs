using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ImgStoWeb.BLL;
using ImgStoWeb.ModelViews;

namespace ImgStoWeb.Services
{
    public class GalSvc : IGalSvc
    {
        private UserBll bll = new UserBll();
        public async Task<bool> AdminLogin(User user)
        {
            if (user == null)
                return false;
            if (string.IsNullOrEmpty(user.Pwd))
                return false;
            if (string.IsNullOrEmpty(user.UserId))
                return false;
            return await bll.AdminLogin(user);
        }
        public bool ConfirmToken(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId))
                return false;
            if (string.IsNullOrEmpty(token))
                return false;
            return   bll.ConfirmToken(userId, token);
        }
        public async Task<string> GetUserToken(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return "";
            return await bll.GetToken(userId);
        }
        public async Task<bool> SignIn(User user)
        {
            if (user is null)
                return false;
            if (string.IsNullOrEmpty(user.UserId))
                return false;
            if (string.IsNullOrEmpty(user.Pwd))
                return false;
            return await bll.ConfirmUser(user);
        }
        public async Task<bool> SignUp(User user)
        {
            if (user is null)
                return false;
            if (string.IsNullOrEmpty(user.UserId))
                return false;
            if (string.IsNullOrEmpty(user.Pwd))
                return false;
            return await bll.CreateUser(user);
        }
        public async Task<User> GetUser(string uid)
        {
            if(string.IsNullOrEmpty(uid))
                return null;
            return await bll.GetUserById(uid);
        }
        public  async Task<bool> ChangePwd(User user)
        {
            if (user == null)
                return false;
            if (string.IsNullOrEmpty(user.Pwd) || string.IsNullOrEmpty(user.UserId))
                return false;
            return await bll.ChangePwd(user);
        }
    }
}