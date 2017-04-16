using SM.Persistence;
using SM.Web.Api.Filter;
using SM.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Linq2Rest;
using System.Data.Entity;
using System.Web.Http.OData.Query;

namespace SM.Web.Api.Controllers
{
    public class ServiceGroupController : BaseController
    {
        [HttpGet]
        [Route("Api/ServiceGroup")]
        public async Task<PagingDataSource<ServiceGroup>> Get(ODataQueryOptions<ServiceGroup> options, [FromUri]ServiceGroupList req)
        {
            ServiceGroupService.DisableProxy();

            var listGroup = ServiceGroupService.GetAll();

            if (req.Includes != null && req.Includes.Length > 0)
            {
                listGroup = Include(listGroup, req.Includes);
            }

            var ls = (IQueryable<ServiceGroup>)options.ApplyTo(listGroup);

            var retVal = new PagingDataSource<ServiceGroup>();

            retVal.Data = await ls.Cast<ServiceGroup>().ToListAsync();
            retVal.Total = Request.GetInlineCount() ?? 0;

            return retVal;
        }

        [HttpGet]
        [Route("Api/ServiceGroup/{Id:int}")]
        public async Task<ServiceGroup> Get(int Id)
        {
            return new ServiceGroup();
        }

        [HttpPost]
        [Route("Api/ServiceGroup")]
        public async Task<ServiceGroup> Post(ServiceGroup serviceGroup)
        {
            if (ModelState.IsValid)
            {
                await  ServiceGroupService.AddAsync(serviceGroup);
            }

            return serviceGroup;
        }

        [HttpPut]
        [Route("Api/ServiceGroup")]
        public async Task<ServiceGroup> Put(ServiceGroup serviceGroup)
        {
            if (ModelState.IsValid)
            {
                await ServiceGroupService.UpdateAsync(serviceGroup);
            }

            return serviceGroup;
        }

        [HttpDelete]
        [Route("Api/ServiceGroup")]
        public async Task<object> Delete(ServiceGroup serviceGroup)
        {
            return new ServiceGroup();
        }
    }
}
