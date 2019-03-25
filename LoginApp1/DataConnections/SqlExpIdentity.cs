using LoginApp1.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LoginApp1.DataConnections
{
    public class SqlExpIdentity : IdentityDbContext<AppUser, AppRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public SqlExpIdentity() : base("LoginApp1") { }
        //{ this.Database.Connection.ConnectionString = GetSqlConnectionStr(); }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.HasDefaultSchema("Security");
        //}

        //private static string GetSqlConnectionStr()
        //{
        //    var c1 = ConfigurationManager.ConnectionStrings["AdWorks16Exp"];
        //    return c1.ConnectionString;
        //}
    }
}