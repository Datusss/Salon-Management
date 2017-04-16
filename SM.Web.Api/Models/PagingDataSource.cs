using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SM.Web.Api.Models
{
    public class PagingDataSource<T> where T : class
    {
        public IList<T> Data { get; set; }
        public long Total { get; set; }
    }

    public class PagingWithTotalDataSource<T> where T : class
    {
        public IList<T> Data { get; set; }
        public long Total { get; set; }
        public double TotalValue { get; set; }
    }
}