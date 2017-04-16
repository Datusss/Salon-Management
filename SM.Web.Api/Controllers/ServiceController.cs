using SM.Persistence;
using SM.Services.Common;
using SM.Web.Api.Filter;
using SM.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.OData.Query;

namespace SM.Web.Api.Controllers
{
    public class ServiceList
    {
        public string[] Includes { get; set; }
    }

    public class ServiceForCustomer
    {
        public string[] Includes { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
    }

    public class ServiceController : BaseController
    {
        [HttpGet]
        [Route("Api/Service")]
        public async Task<PagingDataSource<Service>> Get(ODataQueryOptions<Service> options, [FromUri]ServiceList req)
        {
            ServiceService.DisableProxy();

            var listService = ServiceService.GetAll();

            if (req != null)
            {
                if(req.Includes != null && req.Includes.Length > 0)
                {
                    listService = Include(listService, req.Includes);
                }
            }

            var ls = (IQueryable<Service>)options.ApplyTo(listService);

            var retVal = new PagingDataSource<Service>();

            retVal.Data = await ls.Cast<Service>().ToListAsync();
            retVal.Total = Request.GetInlineCount() ?? 0;

            return retVal;
        }

        [HttpGet]
        [Route("api/service/get-service-for-customer")]
        public async Task<Service> GetServiceForCustomer([FromUri]ServiceForCustomer req)
        {
            ServiceService.DisableProxy();

            var service = await ServiceService.GetServiceForCustomerAsync(req.CustomerId, req.ServiceId);

            return service;
        }

        [HttpPost]
        [Route("Api/Service")]
        public async Task<Service> Post(Service service)
        {
            if (ModelState.IsValid)
            {
                await ServiceService.AddAsync(service);
            }

            return service;
        }

        [HttpPut]
        [Route("Api/Service")]
        public async Task<Service> Put(Service service)
        {
            if (ModelState.IsValid)
            {
                await ServiceService.UpdateAsync(service);
            }

            return service;
        }

        [HttpDelete]
        [Route("Api/Service/{serviceId:int}")]
        public async Task<object> Delete(int serviceId)
        {
            if (serviceId > 0)
            {
                var service = await ServiceService.GetAll().Where(o => o.ServiceId == serviceId).FirstOrDefaultAsync();

                if (service != null)
                {
                    await ServiceService.DeleteAsync(service);
                }
                else
                {
                    throw new ApiException("Dịch vụ không tồn tại");
                }
            }
            else
            {
                throw new ApiException("Dịch vụ không tồn tại");
            }

            return new { Message = "Xóa Dịch vụ thành công" };
        }
    }
}
