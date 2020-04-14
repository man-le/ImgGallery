using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgStoApi.Models
{
    public class tblCategory
    {
        public ObjectId _id { get; set; }
        public string CatId { get; set; }
        public string CatName { get; set; }
        public List<tblCategory> SubCat { get; set; }
    }
}