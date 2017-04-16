using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SM.Web.Api.Filter
{
    public class StaffList
    {
        public string[] Includes { get; set; }
        public int PositionId { get; set; }
    }
}