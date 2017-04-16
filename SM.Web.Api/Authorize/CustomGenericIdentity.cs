using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace SM.Web.Api.Authorize
{
    public class CustomGenericIdentity: GenericIdentity
    {
        public string UserData { get; set; }

        public CustomGenericIdentity(string a, string b) : base(a, b)
        {

        }
    }
}