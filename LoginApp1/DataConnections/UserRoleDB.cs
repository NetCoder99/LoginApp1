using LoginApp1.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LoginApp1.DataConnections
{
    public class UserRoleDB : IdentityDbContext
    {
        public UserRoleDB() : base("LoginApp1") { }
        public DbSet<AppRole> IdentityRoles { get; set; }
    }
}