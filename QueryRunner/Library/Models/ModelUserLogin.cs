using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace Library.Models
{
    public class ModelUserLogin
    {
        public int LoginID { get; set; }
        public string Username { get; set; }
        public DateTime LoginTimestamp { get; set; }
        public DateTime LogoutTimestamp { get; set; }
        public string PCNumber { get; set; }
        public bool UserLoginActive { get; set; }

        internal IQueryable<ModelUserLogin> Get(QueryRunnerEntities context)
        {
            return from userlogin in context.UserLogins
                   select new ModelUserLogin
                   {
                       LoginID = userlogin.LoginID,
                       Username = userlogin.Username,
                       LoginTimestamp = userlogin.LoginTimestamp,
                       LogoutTimestamp = userlogin.LogoutTimestamp,
                       PCNumber = userlogin.PCNumber,
                       UserLoginActive = userlogin.UserLoginActive
                   };
        }

        public UserLogin ToEntity()
        {
            return new UserLogin
            {
                //LoginID = LoginID,
                Username = Username,
                LoginTimestamp = LoginTimestamp,
                LogoutTimestamp = LogoutTimestamp,
                PCNumber = PCNumber,
                UserLoginActive = UserLoginActive
            };
        }

        public void Update(UserLogin userlogin)
        {
            //userlogin.LoginID = LoginID;
            userlogin.Username = Username;
            userlogin.LoginTimestamp = LoginTimestamp;
            userlogin.LogoutTimestamp = LogoutTimestamp;
            userlogin.PCNumber = PCNumber;
            userlogin.UserLoginActive = UserLoginActive;
        }
    }
}
