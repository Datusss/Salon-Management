using SM.Persistence;
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
    public class PositionList
    {
        public string[] Includes { get; set; }
    }

    public class PositionController : BaseController
    {
        [HttpGet]
        [Route("api/staff-position")]
        public async Task<PagingDataSource<Position>> Get(ODataQueryOptions<Position> options, [FromUri]PositionList req)
        {
            PositionService.DisableProxy();

            var litPosition = PositionService.GetAll();

            if (req != null)
            {
                if (req.Includes != null && req.Includes.Length > 0)
                {
                    litPosition = Include(litPosition, req.Includes);
                }
            }

            var ls = (IQueryable<Position>)options.ApplyTo(litPosition);

            var retVal = new PagingDataSource<Position>();

            retVal.Data = await ls.Cast<Position>().ToListAsync();
            retVal.Total = Request.GetInlineCount() ?? 0;

            return retVal;
        }
    }
}
