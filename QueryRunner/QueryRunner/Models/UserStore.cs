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
    public class UserStore<TUser> : IUserStore<TUser, string>,
                                    IUserPasswordStore<TUser, string>,
                                    IQueryableUserStore<TUser, string>
    where TUser : ApplicationUser
    {
        private QueryRunnerEntities _context;

        public IQueryable<TUser> Users
        {
            get
            {
                return _context.Users.Select(u => new ApplicationUser
                {
                    Id = u.Username,     
                    UserName = u.Username,
                    PasswordHash = u.PasswordHash,
                    UserActive = u.UserActive
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

        Task IUserStore<TUser, string>.CreateAsync(TUser user)
        {
            throw new NotImplementedException();
        }
        public Task CreateAsync(TUser user, String username)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(TUser user)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public Task<TUser> FindByIdAsync(string userID)
        {
            StoreUser _store = new StoreUser(_context);
            var user = _store.GetUser(userID);
            var result = new ApplicationUser(user);
            return Task.FromResult(result as TUser);
        }
        public Task<TUser> FindByNameAsync(string userName)
        {
            StoreUser _store = new StoreUser(_context);
            var user = _store.GetUser(userName);
            var result = new ApplicationUser(user);
            return Task.FromResult(result as TUser);
        }
        public Task UpdateAsync(TUser user)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Implements IUserPasswordStore<TUser, Guid>         
        public Task SetPasswordHashAsync(TUser user, string passwordHash)
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