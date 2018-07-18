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
    public class StoreUser : IUser
    {
        private QueryRunnerEntities _ctx;

        public StoreUser() { }
        public StoreUser(QueryRunnerEntities context)
        {
            _ctx = context ?? throw new ArgumentNullException("context");
        }

        public IQueryable<ModelUser> ReadUsers()
        {
            var model = new ModelUser();
            return model.Get(_ctx);
        }    

        public ModelUser GetUser(string username)
        {
            var model = new ModelUser();
            return model.Get(_ctx).FirstOrDefault(x => x.Username == username);
        }

        public void CreateUser(ModelUser model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.UserActive = true;     

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

        public void CreateUser(ModelUser model, string rolename)
        {
            if (string.IsNullOrWhiteSpace(rolename))
                throw new Exception("An error occured and role could not be identified.");
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.UserActive = true;     

                    var entity = model.ToEntity();

                    User user = _ctx.Insert(entity);
                    _ctx.SaveChanges();

                    (new StoreUserRole(_ctx)).CreateUserRole(new ModelUserRole(), user.Username, rolename);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            }
        }

        public void DeleteUser(ModelUser model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.Users.FirstOrDefault(x => x.Username == model.Username);

                    entity.UserActive = false;

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

        public void UpdateUser(ModelUser model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.Users.FirstOrDefault(x => x.Username == model.Username);

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
