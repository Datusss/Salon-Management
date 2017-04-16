using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Persistence
{
    public partial class BillService
    {
        public string Name
        {
            get
            {
                return Service != null ? Service.Name : string.Empty;
            }
        }
    }
}
