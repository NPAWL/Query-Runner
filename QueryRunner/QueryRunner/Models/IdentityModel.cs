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
    public class ApplicationUser : IdentityUser<int, IdentityUserLogin<int>, IdentityUserRole<int>, IdentityUserClaim<int>>, IUser<int>
    {
        private int _id;
        public string Name { get; set; }
        public bool UserActive { get; set; }

        int IUser<int>.Id { get { return _id; } }

        public ApplicationUser() { }
        public ApplicationUser(ModelUser user)
        {
            if (user != null)
            {
                _id = user.UserID;
                Id = user.UserID;
                Name = user.FirstName;
                Email = user.Surname;
                UserName = user.Username;
                PasswordHash = user.Password;
                UserActive = user.UserActive ?? true;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim(ClaimTypes.GivenName, Name));
            return userIdentity;
        }

        public ModelUser ToLibraryUser()
        {
            return new ModelUser
            {
                UserID = Id,
                FirstName = Name,
                Surname = Email,
                Username = UserName,
                Password = PasswordHash,
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