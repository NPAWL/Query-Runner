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
        IQueryable<ModelUserLogin> ReadUserLogin();
        ModelUserLogin GetUserLogin(int loginid);
        void CreateUserLogin(ModelUserLogin model);
        void DeleteUserLogin(ModelUserLogin model);
        void UpdateUserLogin(ModelUserLogin model);
    }
}
