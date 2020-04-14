using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgStoApi.Models
{
    public class tblApiUser
    {
        public ObjectId _id { get; set; }
        public string Uid { get; set; }
        public string Pwd { get; set; }
        public string Token { get; set; }
        public bool IsAd { get; set; }
    }
}