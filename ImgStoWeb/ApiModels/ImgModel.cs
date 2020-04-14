using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgStoWeb.ApiModels
{
    public class ImgModel
    {
        public string ImgId { get; set; }
        public string ImgURL { get; set; }
        public string ImgName { get; set; }
        public string UploadDate { get; set; }
        public string UploadBy { get; set; }
        public string CategoryName { get; set; }
        public int Views { get; set; }
        public int LikeCount { get; set; }
    }
}