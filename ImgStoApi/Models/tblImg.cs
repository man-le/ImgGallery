using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgStoApi.Models
{
    public class tblImg
    {
        public ObjectId _id { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedDate { get; set; }
        public string ImgId { get; set; }
        public string ImgName { get; set; }
        public bool IsValid { get; set; }
        public int View { get; set; }
        public string FileName { get; set; }
        public tblCategory Cat { get; set; }
    }
}