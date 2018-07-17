using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace Library.Models
{
    public class ModelRole
    {
        public string RoleName { get; set; }
        public bool RoleActive { get; set; }

        internal IQueryable<ModelRole> Get(QueryRunnerEntities context)
        {
            return from role in context.Roles
                   select new ModelRole
                   {
                       RoleName = role.RoleName,
                       RoleActive = role.RoleActive
                   };
        }

        public Role ToEntity()
        {
            return new Role
            {
                RoleName = RoleName,
                RoleActive = RoleActive
            };
        }

        public void Update(Role role)
        {
            role.RoleName = RoleName;
            role.RoleActive = RoleActive;
        }
    }
}
