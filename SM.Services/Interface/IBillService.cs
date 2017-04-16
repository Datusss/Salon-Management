using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.Persistence;

namespace SM.Services.Interface
{
    public interface IBillService : IBaseService<Bill>
    {
        //void CreateOrUpdate(Bill bill);
        Task<double> GetTotalCostByCustomerIdAsync(long CustomerId);
    }
}
