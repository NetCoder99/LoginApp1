﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginApp1.Models.Account
{
    public class AppRoleView
    {
        public string key { get; set; }
        public List<AppRole> appRoles { get; set; }
    }
}