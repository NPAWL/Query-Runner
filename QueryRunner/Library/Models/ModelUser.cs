using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace Library.Models
{
    public class ModelUser
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public bool UserActive { get; set; }

        internal IQueryable<ModelUser> Get(QueryRunnerEntities context)
        {
            return from user in context.Users
                   select new ModelUser
                   {
                       Username = user.Username,
                       PasswordHash = user.PasswordHash,
                       UserActive = user.UserActive
                   };
        }

        public User ToEntity()
        {
            return new User
            {
                Username = Username,
                PasswordHash = PasswordHash,
                UserActive = UserActive
            };
        }

        public void Update(User user)
        {
            user.Username = Username;
            user.PasswordHash = PasswordHash;
            user.UserActive = UserActive;
        }
    }
}
