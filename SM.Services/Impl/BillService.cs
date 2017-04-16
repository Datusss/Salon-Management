using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.Persistence;
using SM.Services.Interface;
using System.Data.Entity;

namespace SM.Services.Impl
{
    public class BillService : BaseService<Bill>, IBillService
    {
        public BillService(ThaiAnhSalonEntities Db)
            :base(Db)
        { }

        //public async void CreateOrUpdate(Bill bill)
        //{
        //    if(bill.Id > 0)
        //    {
        //        UpdateAsync(bill);
        //    }
        //    else
        //    {
        //        AddAsync(bill);
        //    }
        //}

        public async Task<double> GetTotalCostByCustomerIdAsync(long CustomerId)
        {
            double sum = 0;

            var billList = GetAll().Where(o => o.CustomerId == CustomerId).ToList();

            foreach (var bill in billList)
            {
                foreach (var bs in bill.BillServices.ToList())
                {
                    sum += bs.RealPrice ?? 0;
                }
            }

            return sum;
        }
        
    }
}
