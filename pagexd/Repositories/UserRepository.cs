using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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

        public List<RolesVM> GetAllRoles()
        {
            IEnumerable<PageRole> roles;
            roles = _applicationDbContext.Roles.ToList();
            var model = new List<RolesVM>();

            foreach (var m in roles)
            {
                var role = new RolesVM()
                {
                    RoleID = m.Id,
                    RoleName = m.Name,
                    RoleNormalizedName = m.NormalizedName,
                };
                model.Add(role);
            }

            return model;
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
                    AccInfo = m.AccInfo,
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
                AccInfo = user.AccInfo,
            };
            var RoleName = _applicationDbContext.Roles.Where(m => m.Id == userVM.UserRoleId);
            userVM.UserRole = RoleName.FirstOrDefault().NormalizedName;
            return userVM;
        }

        public void AdminUserEdit(UsersVM userVM, RolesVM roleVM, Guid id)
        {
            
        }
        
    }
}
