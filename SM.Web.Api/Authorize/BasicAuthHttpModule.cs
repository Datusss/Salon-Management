using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using SM.Persistence;
using SM.Utilities;
using System.Net.Http.Headers;

namespace SM.Web.Api.Authorize
{
    public class BasicAuthHttpModule: IHttpModule
    {
        private const string Realm = "SM-Api";

        private const char AuthorizationHeaderSeparator = ':';
        private const int UsernameIndex = 0;
        private const int PasswordIndex = 1;
        private const int ExpectedCredentialCount = 2;

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnApplicationAuthenticateRequest;
            context.EndRequest += OnApplicationEndRequest;
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;

            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        private static bool AuthenticateUser(string credentials)
        {
            var encoding = Encoding.GetEncoding("iso-8859-1");
            credentials = encoding.GetString(Convert.FromBase64String(credentials));

            var credentialsArray = credentials.Split(AuthorizationHeaderSeparator);

            var username = credentialsArray[UsernameIndex];
            var password = credentialsArray[PasswordIndex];

            var db = new ThaiAnhSalonEntities();
            var hashPassword = MD5Helper.GetMD5Hash(password);

            var user = db.Users.Where(o => o.UserName.ToLower().Equals(username) && o.Password.Equals(hashPassword)).FirstOrDefault();

            if (user != null)
            {
                var indentity = new GenericIdentity(username);
                SetPrincipal(new GenericPrincipal(indentity, null));
                HttpContext.Current.Application["UserSession"] = user as User;

                return true;
            }

            return false;
        }

        private static void OnApplicationAuthenticateRequest(object sender, EventArgs e)
        {
            var request = HttpContext.Current.Request;

            var authHeader = request.Headers["Authorization"];

            if (authHeader != null)
            {
                var authHeaderVal = AuthenticationHeaderValue.Parse(authHeader);

                if (authHeaderVal.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) && authHeaderVal.Parameter != null)
                {
                    AuthenticateUser(authHeaderVal.Parameter);
                }
            }
        }

        private static void OnApplicationEndRequest(object sender, EventArgs e)
        {
            var response = HttpContext.Current.Response;
            if (response.StatusCode == 401)
            {
                response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", Realm));
            }
        }

        public void Dispose()
        { }
    }
}