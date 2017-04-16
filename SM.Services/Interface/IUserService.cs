using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM.Persistence;

namespace SM.Services.Interface
{
    public interface IUserService : IBaseService<User>
    {
        Task<User> GetUserByUsernameAndPassword(string Username, string Password);
    }
}
