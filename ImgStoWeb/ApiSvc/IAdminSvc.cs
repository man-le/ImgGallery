using ImgStoWeb.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ImgStoWeb.ApiSvc
{
    public interface IAdminSvc
    {
        Task<int> GetNumberOfUser();
        List<int> GetChartData(List<ImgModel> models);
    }
}