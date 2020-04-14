using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgStoWeb.Models
{
    public class tblUser
    {
        public ObjectId _id { get; set; }
        public string UserId { get; set; }
        public string Uid { get; set; }
        public string Salt { get; set; }
        public string Pwd { get; set; }
        public bool IsAdmin { get; set; }
        public string Token { get; set; }
        public string LastOnline { get; set; }
    }
}