using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.Persistence;
using SM.Services.Interface;
using System.Data.Entity;

namespace SM.Services.Impl
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(ThaiAnhSalonEntities Db) 
            : base(Db)
        { }

        public async Task<User> GetUserByUsernameAndPassword(string Username, string Password)
        {
            var user = await (from u in GetAll()
                              where u.UserName.ToLower().Equals(Username) && u.Password.Equals(Password)
                              select u).FirstOrDefaultAsync();

            return user;
        }

    }
}
