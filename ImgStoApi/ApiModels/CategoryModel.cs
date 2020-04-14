using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgStoApi.ApiModels
{
    public class CategoryModel
    {
        public string CategoryName { get; set; }
        public string CategoryId { get; set; }
        public List<string> SubCategoryName { get; set; }
    }
}