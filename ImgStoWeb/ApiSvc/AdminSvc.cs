using DoctorForum.Utils;
using ImgStoWeb.ApiModels;
using ImgStoWeb.BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ImgStoWeb.ApiSvc
{
    public class AdminSvc : IAdminSvc
    {
        private UserBll bll = new UserBll();

        public List<int> GetChartData(List<ImgModel> models)
        {
            List<int> rs = new List<int>();
            for(int i = 1; i <= 12; i++)
            {
                rs.Add(0);
            }
            foreach(var item in models)
            {
                switch(DateTime.Parse(item.UploadDate).Month)
                {
                    case 1:
                        rs[0] += 1;
                        break;
                    case 2:
                        rs[1] += 1;
                        break;
                    case 3:
                        rs[2] += 1;
                        break;
                    case 4:
                        rs[3] += 1;
                        break;
                    case 5:
                        rs[4] += 1;
                        break;
                    case 6:
                        rs[5] += 1;
                        break;
                    case 7:
                        rs[6] += 1;
                        break;
                    case 8:
                        rs[7] += 1;
                        break;
                    case 9:
                        rs[10] += 1;
                        break;
                    case 10:
                        rs[9] += 1;
                        break;
                    case 11:
                        rs[10] += 1;
                        break;
                    case 12:
                        rs[11] += 1;
                        break;
                }
            }
            return rs;
        }

        public async Task<int> GetNumberOfUser()
        {
            return await bll.GetNumberUser();
        }
    }
}