using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using DataLayer.Entities;
using Library.Stores;

namespace QueryRunner.Models
{
    public class UserStore<TUser> : IUserStore<TUser, int>,
                                    IUserPasswordStore<TUser, int>,
                                    IQueryableUserStore<TUser, int>
    where TUser : ApplicationUser
    {
        private QueryRunnerEntities _context;

        public IQueryable<TUser> Users
        {
            get
            {
                return _context.Users.Select(u => new ApplicationUser
                {
                    Id = u.UserID,
                    Name = u.FirstName,
                    Email = u.Surname,
                    UserName = u.Username,
                    PasswordHash = u.Password,
                    UserActive = u.UserActive ?? true
                } as TUser);
            }
        }

        public UserStore()
          : this(new QueryRunnerEntities())
        {

        }

        public UserStore(QueryRunnerEntities context)
        {
            _context = context;
        }

        #region Implements IUserStore<TUser, Guid>

        System.Threading.Tasks.Task IUserStore<TUser, int>.CreateAsync(TUser user)
        {
            throw new NotImplementedException();
        }
        public System.Threading.Tasks.Task CreateAsync(TUser user, String username)
        {
            throw new NotImplementedException();
        }
        public System.Threading.Tasks.Task DeleteAsync(TUser user)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public Task<TUser> FindByIdAsync(int userID)
        {
            Store_User _store = new Store_User(_context);
            var user = _store.GetUser(userID);
            var result = new ApplicationUser(user);
            return System.Threading.Tasks.Task.FromResult(result as TUser);
        }
        public Task<TUser> FindByNameAsync(string userName)
        {
            Store_User _store = new Store_User(_context);
            var user = _store.GetUserByUsername(userName);
            var result = new ApplicationUser(user);
            return System.Threading.Tasks.Task.FromResult(result as TUser);
        }
        public System.Threading.Tasks.Task UpdateAsync(TUser user)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Implements IUserPasswordStore<TUser, Guid>         
        public System.Threading.Tasks.Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }
        public Task<string> GetPasswordHashAsync(TUser user)
        {
            throw new NotImplementedException();
        }
        public Task<bool> HasPasswordAsync(TUser user)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}