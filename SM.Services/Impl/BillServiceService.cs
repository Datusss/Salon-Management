using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.Persistence;
using SM.Services.Interface;

namespace SM.Services.Impl
{
    public class BillServiceService :BaseService<SM.Persistence.BillService> , IBillServiceService
    {
        public BillServiceService(ThaiAnhSalonEntities Db)
            : base(Db)
        { }
    }
}
