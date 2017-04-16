using SM.Persistence;
using SM.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Services.Impl
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        public CustomerService(ThaiAnhSalonEntities Db)
            : base(Db)
        { }

    }
}
