using GenericMongo.DBConnection;
using GenericMongo.Repositories;
using ImgStoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgStoWeb.Repositories
{
    public class UnitOfWork
    {
        private DBConnection conn = new DBConnection();
        public UnitOfWork()
        {
            conn.databaseName = "dbImgSto";
        }
        public IRepo<tblUser> UserRepo()
        {
            return new RepositoryImp<tblUser>(conn);
        }
    }
}