using LoginApp1.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginApp1.DataConnections
{
    public class UserAccountDB : IdentityDbContext<AppUser>
    {
        public UserAccountDB() : base("LoginApp1")
        { }

        //public DbSet<AppUser> AppUsers { get; set; }
        //public DbSet<UserAccountDeleted> UserDeletes { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<AppUser>().Map(m =>
        //    {
        //        m.MapInheritedProperties();
        //    });

        //    modelBuilder.Entity<UserAccountDeleted>().Map(m =>
        //    {
        //        m.MapInheritedProperties();
        //    });
        //}

    }
}