using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SM.Web.Api.Common
{
    public class ODataReq
    {
        [DataMember(Name = "$orderby")]
        public string Orderby { get; set; }
        [DataMember(Name = "$select")]
        public string Select { get; set; }

        [DataMember(Name = "$filter")]
        public string Filter { get; set; }
        [DataMember(Name = "$skip")]
        public int? Skip { get; set; }

        [DataMember(Name = "$top")]
        public int? Top { get; set; }

        private IDictionary<string, DateTime[]> DateFilterTypes;

        public NameValueCollection GetModelFilter()
        {
            var retVal = new NameValueCollection();
            if (!string.IsNullOrEmpty(Orderby))
            {
                retVal.Add("$orderby", Orderby);
            }
            if (!string.IsNullOrEmpty(Select))
            {
                retVal.Add("$select", Select);
            }
            if (!string.IsNullOrEmpty(Filter))
            {
                retVal.Add("$filter", BuilDateFilter(Filter));
            }
            return retVal;
        }

        private string BuilDateFilter(string filter)
        {

            foreach (var keyPair in DateFilterTypes)
            {
                DateTime[] t = null;

                DateFilterTypes.TryGetValue(keyPair.Key, out t);
                if (t != null)
                {
                    string filterPart = null;
                    while (filter.IndexOf(keyPair.Key) > 0)
                    {
                        filterPart = null;
                        filter = SplitFilter(filter, keyPair.Key, out filterPart);
                        if (!string.IsNullOrEmpty(filterPart))
                        {
                            filter = BuilDate(filter, filterPart, t[0], t[1]);
                            //break;
                        }
                    }
                }
            }
            return filter;
        }

        private string SplitFilter(string filter, string key, out string fieldname)
        {

            // filter = filter.ToLower();
            fieldname = string.Empty;
            if (filter.IndexOf(key, StringComparison.OrdinalIgnoreCase) <= -1) return filter;
            filter = filter.Replace("(", " ( ").Replace(")", " ) ");
            var arrFilter = filter.Split(' ').ToList();
            var todayindex = arrFilter.IndexOf(key);
            if (todayindex >= 2)
            {
                fieldname = arrFilter[todayindex - 2];
                arrFilter[todayindex - 2] = "{0}";
                arrFilter.RemoveRange(todayindex - 1, 2);
                filter = string.Join(" ", arrFilter);
            }
            filter = filter.Trim();
            filter = filter.Replace(" ( ", "(").Replace(" ) ", ")");
            return filter;
        }

        private string BuilDate(string filter, string fieldName, DateTime fromTime, DateTime toTime)
        {
            const string strformat = "({0} ge datetime'{1}' and {0} lt datetime'{2}')";
            var value = string.Format(strformat, fieldName, fromTime.ToString("yyyy-MM-ddT00:00:00"), toTime.ToString("yyyy-MM-ddT00:00:00"));

            return string.Format(filter, value);
        }
    }
}