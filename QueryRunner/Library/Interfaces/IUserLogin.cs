using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Models;

namespace Library.Interfaces
{
    public interface IUserLogin
    {
        IQueryable<ModelUserLogin> ReadUserLogins();
        ModelUserLogin GetUserLogin(int loginid);
        IQueryable<ModelUserLogin> GetUserLoginsByUsername(string username);
        void CreateUserLogin(ModelUserLogin model);
        void CreateUserLogin(ModelUserLogin model, string username);
        void DeleteUserLogin(ModelUserLogin model);
        void UpdateUserLogin(ModelUserLogin model);
    }
}
