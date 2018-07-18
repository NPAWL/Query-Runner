using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DataLayer.Entities;
using Library.Models;

namespace QueryRunner.Models
{
    public class ApplicationUser : IdentityUser<string, IdentityUserLogin<string>, IdentityUserRole<string>, IdentityUserClaim<string>>, IUser<string>
    {
        private string _id;                
        public bool UserActive { get; set; }

        string IUser<string>.Id { get { return _id; } }

        public ApplicationUser() { }
        public ApplicationUser(ModelUser user)
        {
            if (user != null)
            {
                _id = user.Username;
                Id = user.Username;     
                UserName = user.Username;
                PasswordHash = user.PasswordHash;
                UserActive = user.UserActive;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim(ClaimTypes.GivenName, UserName));
            return userIdentity;
        }

        public ModelUser ToLibraryUser()
        {
            return new ModelUser
            {                      
                Username = UserName,
                PasswordHash = PasswordHash,
                UserActive = UserActive
            };
        }

    }

    public class ApplicationDbContext : QueryRunnerEntities
    {
        public ApplicationDbContext()
          : base()
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

}