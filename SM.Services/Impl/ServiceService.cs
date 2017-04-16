using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.Persistence;
using SM.Services.Interface;
using System.Data.Entity;
using SM.Services.Common;

namespace SM.Services.Impl
{
    public partial class ServiceService : BaseService<Service>, IServiceService
    {
        public ServiceService(ThaiAnhSalonEntities Db)
            : base(Db)
        { }

        private ICustomerService CustomerService
        {
            get
            {
                return new CustomerService(Db);
            }
        }

        public async Task<Service> GetServiceForCustomerAsync(int CustomerId, int ServiceId)
        {
            var service = await GetAll().Where(o => o.ServiceId == ServiceId).FirstOrDefaultAsync();

            var customer = await CustomerService.GetAll().Where(o => o.CustomerId == CustomerId).FirstOrDefaultAsync();

            if(customer != null && customer.Card != null)
            {
                if(customer.Card.CardServices.Any(o => o.ServiceId == service.ServiceId))
                {
                    var discountService = customer.Card.CardServices.Where(o => o.ServiceId == service.ServiceId).FirstOrDefault();

                    service.Price = discountService.DiscountType == DiscountTypes.Price ? discountService.DiscountPrice : (service.Price / 100) * discountService.DiscountRatio;
                }
            }

            return service;
        }
    }
}
