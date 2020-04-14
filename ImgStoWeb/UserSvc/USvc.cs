using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ImgStoWeb.ApiModels;
using ImgStoWeb.Utils;
using Newtonsoft.Json;

namespace ImgStoWeb.UserSvc
{
    public class USvc : IUSvc
    {
        public async Task<List<CategoryModel>> GetCategories()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}category");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var result = await readStream.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<CategoryModel>>(result);
            }
            catch
            {
                return new List<CategoryModel>();
            }
        }

        public async Task<List<ImgModel>> GetFavImg(string userId)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}user/{userId}/img/fav");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var result = await readStream.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<ImgModel>>(result);
            }
            catch
            {
                return new List<ImgModel>();
            }
        }

        public async Task<List<ImgModel>> GetImgByCat(string catId)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}/category/{catId}/img");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var result = await readStream.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<ImgModel>>(result);
            }
            catch
            {
                return new List<ImgModel>();
            }
        }

        public async Task<bool> IncreaseViews(string imgId)
        {
            using (var client = new HttpClient())
            {
                List<CategoryModel> ls = new List<CategoryModel>();
                var uri = new Uri(PathConfig.API_PATH + $"img/{imgId}/view");
                var json = JsonConvert.SerializeObject(imgId);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }
        public async Task<bool> AddToFav(string userId, string imgId)
        {
            using (var client = new HttpClient())
            {
                List<CategoryModel> ls = new List<CategoryModel>();
                var uri = new Uri(PathConfig.API_PATH + $"user/{userId}/img/{imgId}/fav");
                var json = JsonConvert.SerializeObject(userId);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }

        public async Task<List<ImgModel>> GetMostView()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}img/mostview");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var result = await readStream.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<ImgModel>>(result);
            }
            catch
            {
                return new List<ImgModel>();
            }
        }
        public async Task<List<ImgModel>> GetPopular()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}img/popular");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var result = await readStream.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<ImgModel>>(result);
            }
            catch
            {
                return new List<ImgModel>();
            }
        }

        public async Task<List<ImgModel>> SearchImg(string content)
        {
            try
            {
                bool flag = false;
                if (content.StartsWith("#")) {
                    content = content.Split('#')[1];
                    flag = true;
                }
                HttpWebRequest request;
                if (flag) {
                     request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}img/search/%23{content}");
                }
                else
                {
                    request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}img/search/{content}");
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var result = await readStream.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<ImgModel>>(result);
            }
            catch
            {
                return new List<ImgModel>();
            }
        }
    }
}