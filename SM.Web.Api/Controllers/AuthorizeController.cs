using SM.Persistence;
using SM.Utilities;
using SM.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Linq2Rest;

namespace SM.Web.Api.Controllers
{
    public class AuthorizeController : BaseController
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("Api/Authorize")]
        public async Task<object> Login(LoginModel Login)
        {
            UserService.DisableProxy();

            string HashPassword = MD5Helper.GetMD5Hash(Login.Password);

            var User = await UserService.GetUserByUsernameAndPassword(Login.UserName, HashPassword);

            if (User != null)
                return User;

            return "Đăng nhập thất bại";
        }
    }
}
