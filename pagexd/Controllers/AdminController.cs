using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pagexd.Repositories;
using pagexd.ViewModels;
using X.PagedList.Mvc.Core;
using X.PagedList;
using X.PagedList.Mvc.Common;
using pagexd.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using pagexd.Data;
using Microsoft.EntityFrameworkCore;

namespace pagexd.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUserRepository _userRepository;
        private readonly RoleManager<PageRole> _roleManager;
        private readonly UserManager<PageUser> _userManager;
        private readonly ApplicationDbContext _appdbcontext;

        public AdminController(ApplicationDbContext userconext, IPageRepository pageRepository, IUserRepository userRepository, RoleManager<PageRole> roleManager, UserManager<PageUser> userManager)
        {
            _appdbcontext = userconext;
            _userRepository = userRepository;
            _pageRepository = pageRepository;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ContentList(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var items = _pageRepository.GetAllPostsByCreation().ToPagedList(pageNumber, pageSize);
            return View(items);
        }
        

        public IActionResult UsersList(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var items = _userRepository.GetAllUsersVM().ToPagedList(pageNumber, pageSize);
            return View(items);
        }

        public IActionResult Details(int id)
        {
            var postVM = _pageRepository.GetPostByID(id);
            return View(postVM);
        }

        public IActionResult Edit(int id)
        {
            var postVM = _pageRepository.GetPostByID(id);
            if (postVM == null)
            {
                return NotFound();
            }
            return View(postVM);
        }
        [HttpPost]
        public IActionResult Edit(PostVM postVM, int id)
        {
            if (ModelState.IsValid)
            {
                _pageRepository.AdminEdit(postVM, id);
            }
            return View(postVM);
        }

        public IActionResult EditUser(Guid id)
        {
            
            var userVM = _userRepository.GetUserByID(id);
            if (userVM == null)
            {
                return NotFound();
            }
            var Results = from role in _appdbcontext.Roles
                          select new
                          {
                              role.Id,
                              role.NormalizedName,
                              Checked = ((from ur in _appdbcontext.UserRoles
                                          where (ur.UserId == id) & (ur.RoleId == role.Id)
                                          select ur).Count() > 0)
                          };

            var MyCheckBoxList = new List<UserRoleCheckVM>();

            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new UserRoleCheckVM { Id = item.Id, Name = item.NormalizedName, Checked = item.Checked });
            }

            userVM.Roles = MyCheckBoxList;

            return View(userVM);
        }
        [HttpPost]
        public IActionResult EditUser(UsersVM userVM)
        {
            
            if (ModelState.IsValid)
            {
                var MyUser = _appdbcontext.Users.Find(userVM.UserId);
                MyUser.UserName = userVM.UserName;
                MyUser.Email = userVM.Email;
                MyUser.AccInfo = userVM.AccInfo;

                foreach (var item in _appdbcontext.UserRoles)
                {
                    if (item.UserId == userVM.UserId)
                    {
                        _appdbcontext.Entry(item).State = EntityState.Deleted;
                    }
                }
                foreach (var item in userVM.Roles)
                {
                    if (item.Checked)
                    {
                        _appdbcontext.UserRoles.Add(new IdentityUserRole<Guid> { UserId = userVM.UserId, RoleId = item.Id });
                    }
                }

                _appdbcontext.SaveChanges();


            }
            return View(userVM);
        }

        public IActionResult DetailsUser(Guid id)
        {
            var UserDetailsVM = _userRepository.GetUserByID(id);
            return View(UserDetailsVM);
        }
        public IActionResult Archive(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var items = _pageRepository.GetAllPostsByCreation().ToPagedList(pageNumber, pageSize);
            return View(items);
        }

    }
}