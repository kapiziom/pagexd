using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        UserManager<PageUser> _userManager;
        RoleManager<PageRole> _roleManager;

        public UserRepository(ApplicationDbContext context, UserManager<PageUser> userManager, RoleManager<PageRole> roleManager)
        {
            _applicationDbContext = context;
            _userManager = userManager;
            _roleManager = roleManager;
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

        public UsersVM GetUserForEdit(Guid id)
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

            var Results = from role in _applicationDbContext.Roles
                          select new
                          {
                              role.Id,
                              role.NormalizedName,
                              Checked = ((from ur in _applicationDbContext.UserRoles
                                          where (ur.UserId == userVM.UserId) & (ur.RoleId == role.Id)
                                          select ur).Count() > 0)
                          };

            var MyCheckBoxList = new List<UserRoleCheckVM>();

            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new UserRoleCheckVM { Id = item.Id, Name = item.NormalizedName, Checked = item.Checked });
            }

            userVM.Roles = MyCheckBoxList;
            return userVM;
        }

        public void EditUser(UsersVM userVM)
        {
            var MyUser = _applicationDbContext.Users.Find(userVM.UserId);
            MyUser.UserName = userVM.UserName;
            MyUser.Email = userVM.Email;
            MyUser.AccInfo = userVM.AccInfo;

            foreach (var item in _applicationDbContext.UserRoles)
            {
                if (item.UserId == userVM.UserId)
                {
                    _applicationDbContext.Entry(item).State = EntityState.Deleted;
                }
            }
            foreach (var item in userVM.Roles)
            {
                if (item.Checked)
                {
                    _applicationDbContext.UserRoles.Add(new IdentityUserRole<Guid> { UserId = userVM.UserId, RoleId = item.Id });
                }
            }

            _applicationDbContext.SaveChanges();
        }

    }
}
