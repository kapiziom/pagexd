﻿using System;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using pagexd.Repositories;
using pagexd.ViewModels;
using X.PagedList;

namespace pagexd.Controllers
{
    public class PostController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPageRepository _pageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IStorageAccRepository _storageAccRepository;

        public PostController(IPageRepository pageRepository, IWebHostEnvironment webHosting, IUserRepository userRepository, IStorageAccRepository storageAccRepository)
        {
            _userRepository = userRepository;
            _pageRepository = pageRepository;
            _storageAccRepository = storageAccRepository;
            _hostingEnvironment = webHosting;
        }
      
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var postwithcomments = new PostWithCommentsVM();
            var postVM = _pageRepository.GetPostByID(id);
            if(postVM == null)
            {
                RedirectToAction("Index", "Home");
            }
            var comments = _pageRepository.GetCommentsInContent(postVM.PostID);
            postwithcomments.PostVM = postVM;
            postwithcomments.CommentVMs = comments;
            return View(postwithcomments);
        } 

        [HttpPost]
        [Authorize(Roles = "Administrator,NormalUser")]
        public IActionResult Comment(int id, PostWithCommentsVM postWithCommentsVM)
        {
            postWithCommentsVM.PostVM = _pageRepository.GetPostByID(id);
            var comment = postWithCommentsVM.CommentVM;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            comment.UserID = userId;
            comment.UserName = username;
            _pageRepository.AddComment(comment);
            return View(comment);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator,NormalUser")]
        public IActionResult Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var post = _pageRepository.GetPostByID(id);    
            
            if (post == null)
            {
                return NotFound();
            }            
            if (userId != post.UserID)
            {
                return RedirectToAction("Error");
            }
            return View(post);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator,NormalUser")]
        public IActionResult Edit(int id, PostVM postVM)
        {
            if (ModelState.IsValid)
            {
                _pageRepository.Edit(postVM, id);
            }
            return View(postVM);
        }

        [Authorize]
        [HttpGet("Identity/Account/Manage/YourContent")]
        public IActionResult YourContent(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 10;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = _pageRepository.GetUserContent(userId).ToPagedList(pageNumber, pageSize);
            return View(items);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,NormalUser")]
        public IActionResult Create()
        {
            return View();
        }
        // POST: Auction/Create
        [HttpPost]
        [Authorize(Roles = "Administrator,NormalUser")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PostPhotoVM postPhotoVM)
        {
            var post = postPhotoVM.PostVM;
            var photo = postPhotoVM.PhotoVM;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = User.FindFirstValue(ClaimTypes.Name);
            
            string uniqueFileName = null;
            if (photo.File != null)
            {
                uniqueFileName = Guid.NewGuid().ToString();
                photo.Uri = _storageAccRepository.SavePhoto(photo.File, uniqueFileName);
            }

            photo.Name = uniqueFileName;
            post.UserID = userId;
            post.UserName = username;
            _pageRepository.CreatePost(post, photo);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var checkmodel = _pageRepository.GetPostByID(id);
            if (userId != checkmodel.UserID)
            {
                return RedirectToAction("Error");
            }
            var postVM = _pageRepository.GetPostByID(id);
            return View(postVM);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _pageRepository.Delete(id);
            return RedirectToAction("Index", "Home");
        }

    }
}