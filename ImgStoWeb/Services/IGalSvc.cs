using ImgStoWeb.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgStoWeb.Services
{
    interface IGalSvc
    {
        Task<bool> SignUp(User user);
        Task<bool> SignIn(User user);
        bool ConfirmToken(string userId, string token);
        Task<string> GetUserToken(string userId);
        Task<bool> AdminLogin(User user);
        Task<User> GetUser(string uid);
        Task<bool> ChangePwd(User user);
    }
}
