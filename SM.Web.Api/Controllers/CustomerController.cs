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
    public class CustomerList
    {
        public string[] Includes { get; set; }
    }

    public class CustomerController : BaseController
    {
        [HttpGet]
        [Route("Api/Customer")]
        public async Task<PagingWithTotalDataSource<Customer>> Get(ODataQueryOptions<Customer> options, [FromUri]CustomerList req)
        {
             CustomerService.DisableProxy();

            var listCustomer = CustomerService.GetAll();

            if (req != null)
            {
                if (req.Includes != null && req.Includes.Length > 0)
                {
                    listCustomer = Include(listCustomer, req.Includes);
                }
            }

            var ls = (IQueryable<Customer>)options.ApplyTo(listCustomer);

            var retVal = new PagingWithTotalDataSource<Customer>();

            retVal.Data = await ls.Cast<Customer>().ToListAsync();
            retVal.Total = Request.GetInlineCount() ?? 0;

            //foreach (var item in retVal.Data)
            //{
            //    item.TotalCost = await BillService.GetTotalCostByCustomerIdAsync(item.CustomerId);
            //}

            return retVal;
        }

        [HttpPost]
        [Route("Api/Customer")]
        public async Task<Customer> Post(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await CustomerService.AddAsync(customer);
            }

            return customer;
        }

        [HttpPut]
        [Route("Api/Customer")]
        public async Task<Customer> Put(Customer customer)
        {
            if (ModelState.IsValid)
            {
                await CustomerService.UpdateAsync(customer);
            }

            return customer;
        }

        [HttpDelete]
        [Route("Api/Customer/{customerId:int}")]
        public async Task<object> Delete(int customerId)
        {
            if (customerId > 0)
            {
                var customer = await CustomerService.GetAll().Where(o => o.CustomerId == customerId).FirstOrDefaultAsync();

                if (customer != null)
                {
                    await CustomerService.DeleteAsync(customer);
                }
                else
                {
                    throw new ApiException("Khách hàng không tồn tại");
                }
            }
            else
            {
                throw new ApiException("Khách hàng không tồn tại");
            }

            return new { Message = "Xóa Khách hàng thành công" };
        }
    }
}
