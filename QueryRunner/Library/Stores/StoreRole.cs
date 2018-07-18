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
    public class StoreRole : IRole
    {
        private QueryRunnerEntities _ctx;

        public StoreRole() { }
        public StoreRole(QueryRunnerEntities context)
        {
            _ctx = context ?? throw new ArgumentNullException("context");
        }

        public IQueryable<ModelRole> ReadRoles()
        {
            var model = new ModelRole();
            return model.Get(_ctx);
        }

        public ModelRole GetRole(string rolename)
        {
            var model = new ModelRole();
            return model.Get(_ctx).FirstOrDefault(x => x.RoleName == rolename);
        }

        public void CreateRole(ModelRole model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    model.RoleActive = true;     

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

        public void DeleteRole(ModelRole model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.Roles.FirstOrDefault(x => x.RoleName == model.RoleName);

                    entity.RoleActive = false;

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

        public void UpdateRole(ModelRole model)
        {
            using (var transaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    var entity = _ctx.Roles.FirstOrDefault(x => x.RoleName == model.RoleName);

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
