using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace Library.Models
{
    public class ModelUserRole
    {
        public int URID { get; set; }
        public string RoleName { get; set; }
        public string Username { get; set; }
        public bool UserRoleActive { get; set; }

        internal IQueryable<ModelUserRole> Get(QueryRunnerEntities context)
        {
            return from userrole in context.UserRoles
                   select new ModelUserRole
                   {
                       URID = userrole.URID,
                       RoleName = userrole.RoleName,
                       Username = userrole.Username,
                       UserRoleActive = userrole.UserRoleActive
                   };
        }

        public UserRole ToEntity()
        {
            return new UserRole
            {
                //URID = URID,
                RoleName = RoleName,
                Username = Username,
                UserRoleActive = UserRoleActive
            };
        }

        public void Update(UserRole userrole)
        {
            //userrole.URID = URID;
            userrole.RoleName = RoleName;
            userrole.Username = Username;
            userrole.UserRoleActive = UserRoleActive;
        }
    }
}
