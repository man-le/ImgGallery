using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ImgStoWeb.ApiModels;
using ImgStoWeb.BLL;
using ImgStoWeb.ModelViews;
using ImgStoWeb.Utils;
using MongoDB.Bson.IO;

namespace ImgStoWeb.ApiSvc
{
    public class ForumSvc : IForumSvc
    {
        private UserBll bll = new UserBll();
        public async Task<string> AuthMe(User user)
        {
            if (string.IsNullOrEmpty(user.UserId) || string.IsNullOrEmpty(user.Pwd))
                return "";
            if(await bll.ConfirmUser(user))
            {
                return await bll.GetToken(user.UserId);
            }
            return "";
        }

        public async Task<User> GetUserByTok(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;
            return await bll.GetUserByTok(token);
        }

        public async Task<List<ImgModel>> GetUserImgs(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}user/{userId}/img");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var result = await readStream.ReadToEndAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImgModel>>(result);
            }
            catch
            {
                return new List<ImgModel>();
            }
        }

        public async Task<bool> ValidateToken(string tok)
        {
            if (string.IsNullOrEmpty(tok))
                return false;
            return await bll.ValidateToken(tok);
        }
    }
}