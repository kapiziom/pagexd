using System;
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

        public PostController(IPageRepository pageRepository, IWebHostEnvironment webHosting, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _pageRepository = pageRepository;
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

        [Authorize(Roles = "Administrator,NormalUser")]
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

        [HttpPost]
        [Authorize(Roles = "Administrator,NormalUser")]
        public IActionResult Comment(int id, PostWithCommentsVM postWithCommentsVM)
        {
            postWithCommentsVM.PostVM = _pageRepository.GetPostByID(id);
            var comment = postWithCommentsVM.CommentVM;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            comment.UserID = userId;
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
            post.UserID = userId;

            _pageRepository.CreatePost(post, photo);

            
            return RedirectToAction("Index", "Home");
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator,NormalUser")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _pageRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}