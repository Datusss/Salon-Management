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
    public class StaffController : BaseController
    {
        [HttpGet]
        [Route("Api/Staff")]
        public async Task<PagingDataSource<Staff>> Get(ODataQueryOptions<Staff> options, [FromUri]StaffList req)
        {
            StaffService.DisableProxy();

            var litStaff = StaffService.GetAll();

            if(req != null)
            {
                if(req.Includes != null && req.Includes.Length > 0)
                {
                    litStaff = Include(litStaff, req.Includes);
                }

                if (req.PositionId > 0)
                {
                    litStaff = litStaff.Where(o => o.PositionId == req.PositionId);
                }
            }

            var ls = (IQueryable<Staff>)options.ApplyTo(litStaff);

            var retVal = new PagingDataSource<Staff>();

            retVal.Data = await ls.Cast<Staff>().ToListAsync();
            retVal.Total = Request.GetInlineCount() ?? 0;

            return retVal;
        }

        [HttpPost]
        [Route("Api/Staff")]
        public async Task<Staff> Post(Staff staff)
        {
            if (ModelState.IsValid)
            {
                await StaffService.AddAsync(staff);
            }

            return staff;
        }

        [HttpPut]
        [Route("Api/Staff")]
        public async Task<Staff> Put(Staff staff)
        {
            if (ModelState.IsValid)
            {
                await StaffService.UpdateAsync(staff);
            }

            return staff;
        }

        [HttpDelete]
        [Route("Api/Staff/{staffId:int}")]
        public async Task<object> Delete(int staffId)
        {
            if (staffId > 0)
            {
                var staff = await StaffService.GetAll().Where(o => o.StaffId == staffId).FirstOrDefaultAsync();

                if (staff != null)
                {
                    await StaffService.DeleteAsync(staff);
                }
                else
                {
                    throw new ApiException("Nhân viên không tồn tại");
                }
            }
            else
            {
                throw new ApiException("Nhân viên không tồn tại");
            }

            return new { Message = "Xóa Nhân viên thành công" };
        }
    }
}
