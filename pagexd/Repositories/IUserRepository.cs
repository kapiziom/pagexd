using pagexd.Data;
using pagexd.Models;
using pagexd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Repositories
{
    public interface IUserRepository
    {

        List<PageUser> GetAllUsers();

        List<UsersVM> GetAllUsersVM();

        UsersVM GetUserByID(Guid id);
    }
}
