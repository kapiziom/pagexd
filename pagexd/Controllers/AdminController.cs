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

namespace pagexd.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUserRepository _userRepository;

        public AdminController(IPageRepository pageRepository, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _pageRepository = pageRepository;
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
        [HttpGet("Admin/Delete/{id}")]
        public IActionResult ContentDelete(int id)
        {
            
            var postVM = _pageRepository.GetPostByID(id);
            return View(postVM);
        }

        [HttpPost, ActionName("ContentDelete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _pageRepository.Delete(id);
            return RedirectToAction(nameof(Index));
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

        public IActionResult UserDetails(Guid id)
        {
            var UserDetailsVM = _userRepository.GetUserByID(id);
            return View(UserDetailsVM);
        }
    }
}