using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgStoWeb.ApiModels
{
    public class SubCatUpload
    {
        public string FatherCat { get; set; }
        public List<SubCatModel> Subs { get; set; }
    }
    public class SubCatModel
    {
        public string CatId { get; set; }
        public string CatName { get; set; }
    }
}