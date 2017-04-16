using SM.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SM.Persistence;
using System.Web;

namespace SM.Web.Api.Controllers
{
    [Authorize]
    public class BaseController : ApiController
    {
        #region Register service
        public IUserService UserService { get; set; }
        public IBillService BillService { get; set; }
        public ICustomerService CustomerService { get; set; }
        public IPostionService PositionService { get; set; }
        public IStaffService StaffService { get; set; }
        public IServiceGroupService ServiceGroupService { get; set; }
        public IServiceService ServiceService { get; set; }
        public IBillServiceService BillServiceService { get; set; }
        #endregion

        protected User CurrentUser
        {
            get
            {
                var user = HttpContext.Current.Application["UserSession"] as User;

                return user;
            }
        }

        /// <summary>
        /// Eager load a relation
        /// </summary>
        /// <typeparam name="TEntity">An EF entity type</typeparam>
        /// <param name="entitySet">IQueryable object </param>
        /// <param name="relations">List of relationship to eager load</param>
        /// <returns>Updated IQueryable set </returns>
        protected virtual IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> entitySet, string[] relations) where TEntity : class
        {
            if (relations != null)
            {
                foreach (var relation in relations)
                {
                    entitySet = entitySet.Include(relation);
                }
            }
            return entitySet;
        }
    }
}
