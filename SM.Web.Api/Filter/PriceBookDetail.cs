using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SM.Web.Api.Filter
{
    public class PriceBookDetail
    {
        public string[] Includes { get; set; }
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
    }
}