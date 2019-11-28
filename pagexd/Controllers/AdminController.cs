using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pagexd.Repositories;
using pagexd.ViewModels;

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
        public IActionResult ContentList()
        {
            var items = _pageRepository.GetAllPosts();

            return View(items);
        }
        

        public IActionResult UsersList()
        {
            var items = _userRepository.GetAllUsers();
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
                _pageRepository.Edit(postVM, id);
            }
            return View(postVM);
        }


    }
}