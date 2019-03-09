using LoginApp1.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginApp1.Models
{
    public class BaseViewModel
    {
        public String AppTitle { get; set; }
        public String PageTitle { get; set; }
        public String MetaKeywords { get; set; }
        public String MetaDescription { get; set; }

        public BaseViewModel()
        {
            var layoutType = typeof(BaseViewModel);
            var nameSpace = layoutType.Namespace;
            AppTitle = nameSpace;
        }
    }
}