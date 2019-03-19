﻿using LoginApp1.Models.Account;
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
        public UserAccountDB() : base("LoginApp1"){ }

        // DbSet<AppRole> IdentityUsers { get; set; }
    }
}