using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Interfaces
{
    public interface IRole
    {
        IQueryable<ModelRole> ReadRoles();
        ModelRole GetRole(string rolename);
        void CreateRole(ModelRole model);
        void DeleteRole(ModelRole model);
        void UpdateRole(ModelRole model);
    }
}
