using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Persistence
{
    public partial class Staff
    {
        public string PositionName
        {
            get
            {
                return Position != null ? Position.Name : string.Empty;
            }
        }
    }
}
