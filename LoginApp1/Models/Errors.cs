using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginApp1.Models
{
    public class Errors
    {
        public Exception last_error { get; set; }
        public string error_message { get; set; }

        public Errors()
        {}

        public Errors(Exception crnt_error)
        {
            last_error = crnt_error;
        }

    }
}