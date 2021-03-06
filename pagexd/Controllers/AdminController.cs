﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pagexd.Repositories;
using pagexd.ViewModels;
using X.PagedList;

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
            return View(userVM);
        }
        [HttpPost]
        public IActionResult EditUser(UsersVM userVM)
        {
            if (ModelState.IsValid)
            {
                _userRepository.EditUser(userVM);
            }
            return RedirectToAction("UsersList", "Admin");
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