using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using SM.Web.Api.Models;
using SM.Persistence;
using SM.Web.Api.Filter;
using System.Web.Http.OData.Query;
using System.Data.Entity;
using SM.Services.Common;

namespace SM.Web.Api.Controllers
{
    public class GetBill
    {
        public string[] Includes { get; set; }
    }

    public class BillController : BaseController
    {
        [HttpGet]
        [Route("Api/Bill")]
        public async Task<PagingDataSource<Bill>> Get(ODataQueryOptions<Bill> options,[FromUri]BillList req)
        {
            BillService.DisableProxy();

            var listBill = BillService.GetAll();

            if (req != null && req.Includes != null && req.Includes.Length > 0)
            {
                listBill = Include(listBill, req.Includes);
            }

            var ls = (IQueryable<Bill>)options.ApplyTo(listBill);

            var retVal = new PagingDataSource<Bill>();

            retVal.Data = await ls.Cast<Bill>().ToListAsync();
            retVal.Total = Request.GetInlineCount() ?? 0;

            return retVal;
        }


        [HttpGet]
        [Route("api/bill/{billId:int}")]
        public async Task<object> Get(int billId, [FromUri]GetBill req)
        {
            BillService.DisableProxy();

            var bill = BillService.GetAll().Where(o => o.Id == billId);

            if(req != null)
            {
                if (req.Includes != null && req.Includes.Length > 0)
                    bill = Include(bill, req.Includes);
            }

            var retVal = await bill.FirstOrDefaultAsync();

            if (retVal == null)
                throw new ApiException("Không tồn tại hóa đơn này");

            return retVal;
        }

        [HttpPost]
        [Route("Api/Bill")]
        public async Task<Bill> Post(Bill bill)
        {
            if (ModelState.IsValid)
            {
                bill.CreatedBy = CurrentUser.UserID;
                bill.CreatedDate = DateTime.Now;

                await BillService.AddAsync(bill);
            }

            return bill;
        }

        [HttpPut]
        [Route("Api/Bill")]
        public async Task<Bill> Put(Bill bill)
        {
            if (ModelState.IsValid)
            {
                await BillService.UpdateAsync(bill);
            }

            return bill;
        }

        [HttpDelete]
        [Route("api/bill/{billId:int}")]
        public async Task<object> Delete(int billId)
        {
            if(billId > 0)
            {
                var bill = await BillService.GetAll().Where(o => o.Id == billId).FirstOrDefaultAsync();

                if(bill != null)
                {
                    var lsBs = bill.BillServices.ToList();

                    foreach (var bs in lsBs)
                    {
                        await BillServiceService.DeleteAsync(bs);
                    }

                    await BillService.DeleteAsync(bill);
                }
                else
                {
                    throw new ApiException("Hóa đơn không tồn tại");
                }
            }
            else
            {
                throw new ApiException("Mã hóa đơn không tồn tại");
            }

            return new { Message = "Xóa hóa đơn thành công" };
        }
    }
}
