using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Interfaces
{
    public interface IUserRole
    {
        IQueryable<ModelUserRole> ReadUserRoles();
        ModelUserRole GetUserRole(int urid);
        ModelUserRole GetUserRole(string rolename, string username);
        void CreateUserRole(ModelUserRole model, string rolename, string username);
        void DeleteUserRole(ModelUserRole model);
        void UpdateUserRole(ModelUserRole model);
    }
}
