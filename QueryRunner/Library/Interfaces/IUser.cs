using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Interfaces
{
    public interface IUser
    {
        IQueryable<ModelUser> ReadUsers(); 
        ModelUser GetUser(string username);
        List<ModelUser> GetAdminUsers();
        List<ModelUser> GetStudentUsers();
        void CreateUser(ModelUser model);
        void CreateUser(ModelUser model, string rolename);
        void DeleteUser(ModelUser model);
        void UpdateUser(ModelUser model);
    }
}
