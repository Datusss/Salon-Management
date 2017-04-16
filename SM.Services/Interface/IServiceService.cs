﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.Persistence;

namespace SM.Services.Interface
{
    public partial interface IServiceService :IBaseService<Service>
    {
        Task<Service> GetServiceForCustomerAsync(int CustomerId, int ServiceId);
    }
}
