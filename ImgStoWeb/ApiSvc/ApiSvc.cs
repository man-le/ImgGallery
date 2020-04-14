using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ImgStoWeb.ApiModels;
using ImgStoWeb.Utils;
using Newtonsoft.Json;

namespace ImgStoWeb.ApiSvc
{
    public class ApiSvc : IApiSvc
    {

        public async Task<bool> ConfirmImg(string id)
        {
            using (var client = new HttpClient())
            {
                List<CategoryModel> ls = new List<CategoryModel>();
                var uri = new Uri(PathConfig.API_PATH + $"admin/img/{id}/confirm");
                var json = JsonConvert.SerializeObject(id);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }

        public Task<bool> DeleteCategory(string catId)
        {
            throw new NotImplementedException();
        }

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

        public async Task<ImgModel> GetImgById(string id)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}img/{id}");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                var result = await readStream.ReadToEndAsync();
                return JsonConvert.DeserializeObject<ImgModel>(result);
            }
            catch
            {
                return new ImgModel();
            }
        }

        public async Task<List<ImgModel>> GetImgs()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}img");
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
        public async Task<List<ImgModel>> GetDups(string id)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}img/similar/{id}");
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
        public async Task<List<ImgModel>> GetPendingImg()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{PathConfig.API_PATH}admin/pending/img");
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

        public async Task<bool> PostCategory(CategoryModel category)
        {
            using (var client = new HttpClient())
            {
                List<CategoryModel> ls = new List<CategoryModel>();
                var uri = new Uri(PathConfig.API_PATH + "category");
                var json = JsonConvert.SerializeObject(category);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }

        public async Task<bool> PostImg(ImgModel model)
        {
            using (var client = new HttpClient())
            {
                List<CategoryModel> ls = new List<CategoryModel>();
                var uri = new Uri(PathConfig.API_PATH + "img");
                var json = JsonConvert.SerializeObject(model);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }

        public async Task<bool> PostSubCategory(SubCatUpload cat)
        {
            using (var client = new HttpClient())
            {
                List<CategoryModel> ls = new List<CategoryModel>();
                var uri = new Uri(PathConfig.API_PATH + "category/sub");
                var json = JsonConvert.SerializeObject(cat);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, stringContent);
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }

        public async Task<bool> UploadImg(HttpPostedFileBase file, string fileName)
        {
            try
            {
                var method = new HttpMethod("POST");
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(method, PathConfig.API_PATH + $"img/upload/{fileName}");
                    var content = new MultipartFormDataContent();
                    content.Add(new StreamContent(file.InputStream), "file", file.FileName);
                    request.Content = content;
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteImg(string id)
        {
            using (var client = new HttpClient())
            {
                List<CategoryModel> ls = new List<CategoryModel>();
                var uri = new Uri(PathConfig.API_PATH + "img/?id="+id);
                var response = await client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                    return true;
                return false;
            }
        }
    }
}