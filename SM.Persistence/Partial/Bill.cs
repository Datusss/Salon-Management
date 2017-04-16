using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Persistence
{
    public partial class Bill
    {
        public string CustomerName
        {
            get
            {
                return Customer != null ? Customer.CustomerName : string.Empty;
            }
        }

        public string CustomerPhone
        {
            get
            {
                return Customer != null ? Customer.Phone : string.Empty;
            }
        }

        public string CardType
        {
            get
            {
                return Customer != null ? Customer.Card != null ? Customer.Card.Name : string.Empty : string.Empty;
            }
        }
    }
}
