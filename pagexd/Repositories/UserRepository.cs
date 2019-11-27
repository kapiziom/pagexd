using pagexd.Data;
using pagexd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Repositories
{
    public class UserRepository : IUserRepository
    {

        ApplicationDbContext _applicationDbContext;

        public UserRepository(ApplicationDbContext context)
        {
            _applicationDbContext = context;
        }

        public List<PageUser> GetAllUsers()
        {
            var users = _applicationDbContext.Users.ToList();
            return users;
        }

        
    }
}
