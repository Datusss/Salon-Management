using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Persistence
{
    public partial class Customer
    {
        public double TotalCost
        {
            get
            {
                return Bills != null ? Bills.ToList().Sum(x => x.Price) ?? 0 : 0;
            }
        }
    }
}
