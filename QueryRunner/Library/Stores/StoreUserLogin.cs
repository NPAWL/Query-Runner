using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Entities;
using Library.Interfaces;
using Library.Models;

namespace Library.Stores
{
    public class StoreUserLogin : IUserLogin
    {
        private QueryRunnerEntities _ctx;

        public StoreUserLogin() { }
        public StoreUserLogin(QueryRunnerEntities context)
        {
            _ctx = context ?? throw new ArgumentNullException("context");
        }

        public IQueryable<ModelUserLogin> ReadUserLogins()
        {
            var model = new ModelUserLogin();
            return model.Get(_ctx);
        }

        public ModelUserLogin GetUserLogin(int loginid)
        {
            var model = new ModelUserLogin();
            return model.Get(_ctx).FirstOrDefault(x => x.LoginID == loginid);
        }

        public IQueryable<ModelUserLogin> GetUserLoginsByUsername(string username)
        {
            var model = new ModelUserLogin();
            return model.Get(_ctx).Where(x => x.Username == username);
        }

        public void CreateUserLogin(ModelUserLogin model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.UserLoginActive = true;     

                    var entity = model.ToEntity();

                    _ctx.Insert(entity);
                    _ctx.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

        public void CreateUserLogin(ModelUserLogin model, string username)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.UserLoginActive = true;  
                    model.Username = username;
                    model.LoginTimestamp = DateTime.Now;

                    var entity = model.ToEntity();

                    _ctx.Insert(entity);
                    _ctx.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

        public void DeleteUserLogin(ModelUserLogin model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.UserLogins.FirstOrDefault(x => x.LoginID == model.LoginID);

                    entity.UserLoginActive = false;

                    _ctx.Update(entity);
                    _ctx.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

        public void UpdateUserLogin(ModelUserLogin model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.UserLogins.FirstOrDefault(x => x.LoginID == model.LoginID);

                    model.Update(entity);

                    _ctx.Update(entity);
                    _ctx.SaveChanges();

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

    }
}
