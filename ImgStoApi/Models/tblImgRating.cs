using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgStoApi.Models
{
    public class tblImgRating
    {
        public ObjectId _id { get; set; }
        public string UserName { get; set; }
        public string ImgId { get; set; }
        public bool IsLike { get; set; }
    }
}