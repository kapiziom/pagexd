﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pagexd.Data;
using pagexd.Models;
using pagexd.Repositories;
using pagexd.ViewModels;

namespace pagexd.Controllers
{
    public class PostController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPageRepository _pageRepository;
        private readonly IUserRepository _userRepository;
        private readonly ModelDbContext _context;

        public PostController(IPageRepository pageRepository, IWebHostEnvironment webHosting, IUserRepository userRepository, ModelDbContext context)
        {
            _userRepository = userRepository;
            _pageRepository = pageRepository;
            _hostingEnvironment = webHosting;
            _context = context;
        }
      
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var postVM = _pageRepository.GetPostByID(id);
            return View(postVM);
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var checkmodel = _pageRepository.GetPostByID(id);
            if (userId != checkmodel.UserID)
            {
                return RedirectToAction("Index");
            }
            var postVM = _pageRepository.GetPostByID(id);
            return View(postVM);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            
            if (userId != post.UserID)
            {
                return RedirectToAction("Index");
            }

            return View(post);
        }
        [HttpPost]
        public IActionResult Edit(int id, PostVM postVM)
        {
            //////////do zrobienia
            return View(postVM);
        }

        [Authorize]
        [HttpGet("Identity/Account/Manage/YourContent")]
        public IActionResult YourContent()
        {
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            var items = _pageRepository.GetUserContent(userId);

            return View(items);
        }


        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        // POST: Auction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostPhotoVM postPhotoVM)
        {
            var post = postPhotoVM.PostVM;
            var photo = postPhotoVM.PhotoVM;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            string uniqueFileName = null;
            string photopath = null;
            if (photo.File != null)
            {                
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.File.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                photo.File.CopyTo(new FileStream(filePath, FileMode.Create));
                photopath = filePath;
            }
            
            photo.Name = uniqueFileName;
            photo.PhotoPath = photopath;
            photo.PathForView = "~/images/" + uniqueFileName;
            post.IsAccepted = true;
            post.IsArchived = false;
            post.UserID = userId;

            _pageRepository.CreatePost(post, photo);

            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _pageRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}