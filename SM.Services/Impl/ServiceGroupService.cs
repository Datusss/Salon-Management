using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.Persistence;
using SM.Services.Interface;

namespace SM.Services.Impl
{
    public partial class ServiceGroupService: BaseService<ServiceGroup>, IServiceGroupService
    {
        public ServiceGroupService(ThaiAnhSalonEntities Db)
            : base(Db)
        { }
    }
}
