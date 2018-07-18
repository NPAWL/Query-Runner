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
    public class StoreUserRole : IUserRole
    {
        private QueryRunnerEntities _ctx;

        public StoreUserRole() { }
        public StoreUserRole(QueryRunnerEntities context)
        {
            _ctx = context ?? throw new ArgumentNullException("context");
        }

        public IQueryable<ModelUserRole> ReadUserRoles()
        {
            var model = new ModelUserRole();
            return model.Get(_ctx);
        }

        public ModelUserRole GetUserRole(int urid)
        {
            var model = new ModelUserRole();
            return model.Get(_ctx).FirstOrDefault(x => x.URID == urid);
        }
        
        public ModelUserRole GetUserRole(string username, string rolename)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new Exception("An error occured and user could not be identified.");
            if (string.IsNullOrWhiteSpace(rolename))
                throw new Exception("An error occured and role could not be identified.");
            var model = new ModelUserRole();
            return model.Get(_ctx).FirstOrDefault(x => x.UserRoleActive == true && x.Username == username && x.RoleName == rolename);
        }

        public void CreateUserRole(ModelUserRole model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.UserRoleActive = true; 

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

        public void CreateUserRole(ModelUserRole model, string username, string rolename)
        {
            //using (var transaction = _ctx.Database.BeginTransaction())
            //{
                try
                {
                    model.UserRoleActive = true;  
                    model.Username = username;
                    model.RoleName = rolename;

                    var entity = model.ToEntity();

                    _ctx.Insert(entity);
                    _ctx.SaveChanges();

                    //transaction.Commit();
                }
                catch (Exception e)
                {
                    //transaction.Rollback();
                    throw new ArgumentException(e.Message);
                }
            //}
        }

        public void DeleteUserRole(ModelUserRole model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.UserRoles.FirstOrDefault(x => x.URID == model.URID);

                    entity.UserRoleActive = false;

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

        public void UpdateUserRole(ModelUserRole model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.UserRoles.FirstOrDefault(x => x.URID == model.URID);

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
