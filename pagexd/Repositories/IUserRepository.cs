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
        List<UsersVM> GetAllUsersVM();
        List<RolesVM> GetAllRoles();
        UsersVM GetUserByID(Guid id);
        void EditUser(UsersVM userVM);
    }
}
