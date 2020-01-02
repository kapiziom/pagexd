using pagexd.Data;
using pagexd.Models;
using pagexd.ViewModels;
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

        public List<UsersVM> GetAllUsersVM()
        {
            IEnumerable<PageUser> users;
            users = _applicationDbContext.Users.ToList();
            var model = new List<UsersVM>();

            foreach (var m in users)
            {
                var user = new UsersVM()
                {
                    UserId = m.Id,
                    UserName = m.UserName,
                    Email = m.Email,

                };
                var UserRole = _applicationDbContext.UserRoles.Where(m => m.UserId == user.UserId);
                user.UserRoleId = UserRole.FirstOrDefault().RoleId;
                var RoleName = _applicationDbContext.Roles.Where(m => m.Id == user.UserRoleId);
                user.UserRole = RoleName.FirstOrDefault().NormalizedName;


                model.Add(user);
            }
            return model;
        }

        public UsersVM GetUserByID(Guid id)
        {
            var user = _applicationDbContext.Users.FirstOrDefault(m => m.Id == id);
            if (user == null)
            {
                return null;
            }
            var UserRole = _applicationDbContext.UserRoles.Where(m => m.UserId == id);

            var userVM = new UsersVM()
            {
                UserId = id,
                UserName = user.UserName,
                Email = user.Email,
                UserRoleId = UserRole.FirstOrDefault().RoleId,
            };
            var RoleName = _applicationDbContext.Roles.Where(m => m.Id == userVM.UserRoleId);
            userVM.UserRole = RoleName.FirstOrDefault().NormalizedName;
            return userVM;
        }

    }
}
