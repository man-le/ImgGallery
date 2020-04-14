using GenericMongo.DBConnection;
using GenericMongo.Repositories;
using ImgStoApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImgStoApi.Repositories
{
    public class UnitOfWork
    {
        private DBConnection conn = new DBConnection();
        public UnitOfWork()
        {
            conn.databaseName = "dbImgStoApi";
        }
        public IRepo<tblCategory> CategoryRepo()
        {
            return new RepositoryImp<tblCategory>(conn);
        }
        public IRepo<tblImg> ImageRepo()
        {
            return new RepositoryImp<tblImg>(conn);
        }
        public IRepo<tblApiUser> ApiUserRepo()
        {
            return new RepositoryImp<tblApiUser>(conn);
        }
        public IRepo<tblImgRating> ImgRatingRepo()
        {
            return new RepositoryImp<tblImgRating>(conn);
        }
    }
}