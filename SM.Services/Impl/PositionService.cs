using SM.Persistence;
using SM.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Services.Impl
{
    public class PositionService : BaseService<Position>, IPostionService
    {
        public PositionService(ThaiAnhSalonEntities Db)
            :base(Db)
        { }

    }
}
