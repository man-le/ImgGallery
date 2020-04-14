using DoctorForum.Utils;
using ImgStoWeb.Models;
using ImgStoWeb.ModelViews;
using ImgStoWeb.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ImgStoWeb.BLL
{
    public class UserBll
    {
        private UnitOfWork uow = new UnitOfWork();
        public async Task<bool> CreateUser(User signUp)
        {
            try
            {
                tblUser user = new tblUser()
                {
                    Salt = BCrypt.Net.BCrypt.GenerateSalt(),
                    UserId = signUp.UserId,
                    IsAdmin = false,
                };
                var uid = Guid.NewGuid();
                var pwd = BCrypt.Net.BCrypt.HashPassword(signUp.Pwd, user.Salt);
                byte[] key = uid.ToByteArray();
                string token = Convert.ToBase64String(key.ToArray());
                user.Pwd = pwd;
                user.Uid = uid.ToString();
                user.Token = token;
                var repo = uow.UserRepo();
                List<tblUser> ls = new List<tblUser>();
                ls.Add(user);
                return await repo.AddEntities(ls);
            }
            catch 
            {
                return false;
            }
        }
        public async Task<bool> ConfirmUser(User signIn)
        {
            try
            {
                var repo = uow.UserRepo();
                var filter = Builders<tblUser>.Filter.Eq(x => x.UserId, signIn.UserId);
                var tbl = await repo.CustomQuery(filter);
                if (BCrypt.Net.BCrypt.HashPassword(signIn.Pwd, tbl.First().Salt) == tbl.First().Pwd)
                    return true;
                return false;
            }
            catch
            {
                return false;
                throw;
            }
        }
        public bool ConfirmToken(string uid, string tok)
        {
            try
            {
                var repo = uow.UserRepo();
                var filter = Builders<tblUser>.Filter.Eq(x => x.UserId, uid);
                var rs = repo.CustomQueryNor(filter);
                if (rs.First().Token.Equals(tok))
                    return true;
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public async Task<string> GetToken(string uid)
        {
            try
            {
                var repo = uow.UserRepo();
                var filter = Builders<tblUser>.Filter.Eq(x => x.UserId, uid);
                var rs = await repo.CustomQuery(filter);
                return rs.First().Token;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public async Task<bool> AdminLogin(User user)
        {
            try
            {
                var repo = uow.UserRepo();
                bool flag = false;
                foreach (var item in await repo.GetEntities())
                {
                    if (item.UserId == user.UserId
                        && BCrypt.Net.BCrypt.HashPassword(user.Pwd, item.Salt) == item.Pwd
                        && item.IsAdmin)
                    {
                        flag = true;
                        break;
                    }
                }
                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<User> GetUserByTok(string token)
        {
            try
            {
                var repo = uow.UserRepo();
                var filter = Builders<tblUser>.Filter.Eq(x => x.Token, token);
                var rs = await repo.CustomQuery(filter);
                return new User()
                {
                    UserId = rs.First().UserId
                };
            }
            catch (Exception)
            {
                return null;

            }
        }
        public async Task<bool> ValidateToken(string token)
        {
            try
            {
                var repo = uow.UserRepo();
                bool flag = false;
                foreach (var item in await repo.GetEntities())
                {
                    if (item.Token == token)
                    {
                        flag = true;
                        break;
                    }
                }
                return flag;
            }
            catch
            {
                return false;
            }
        }
        public async Task<User> GetUserById(string uid)
        {
            try
            {
                var repo = uow.UserRepo();
                foreach(var item in await repo.GetEntities())
                {
                    if (item.UserId == uid)
                        return new User()
                        {
                            UserId = uid,
                            Pwd = "***************"
                        };
                }
                return new User();
            }
            catch (Exception)
            {
                return new User();
                throw;
            }
        }
        public async Task<bool> ChangePwd(User user)
        {
            try
            {
                var repo = uow.UserRepo();
                tblUser temp = null;
                foreach(var item in await repo.GetEntities())
                {
                    if (item.UserId == user.UserId)
                    {
                        temp = item;
                        break;
                    }
                }
                if(null == temp)
                {
                    return false;
                }
                temp.Salt = BCrypt.Net.BCrypt.GenerateSalt();
                temp.Pwd = BCrypt.Net.BCrypt.HashPassword(user.Pwd.Trim(), temp.Salt);
                var GUID = Guid.NewGuid();
                byte[] key = GUID.ToByteArray();
                string token = Convert.ToBase64String(key.ToArray());
                temp.Uid = GUID.ToString();
                temp.Token = token;
                var filter = Builders<tblUser>.Filter.Eq(x => x.UserId, temp.UserId);
                return await repo.UpdateEntities(filter, temp);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public async Task<int> GetNumberUser()
        {
            try
            {
                var repo = uow.UserRepo();
                var rs = await repo.GetEntities();
                return rs.Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
        
    }
}