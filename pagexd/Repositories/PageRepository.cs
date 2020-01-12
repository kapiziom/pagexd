using pagexd.Data;
using pagexd.Models;
using pagexd.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly ModelDbContext _context;
        private readonly ApplicationDbContext _identitycontext;

        public PageRepository(ModelDbContext modelDbContext, ApplicationDbContext applicationDbContext)
        {
            _context = modelDbContext;
            _identitycontext = applicationDbContext;
        }
 
        public void Edit(PostVM postVM, int id)
        {
            var post = _context.Posts.FirstOrDefault(m => m.PostID == id);
            post.Title = postVM.Title;
            post.Txt = postVM.Txt;
            _context.SaveChanges();
        }
        public void AdminEdit(PostVM postVM, int id)
        {
            var post = _context.Posts.FirstOrDefault(m => m.PostID == id);
            post.Title = postVM.Title;
            post.Txt = postVM.Txt;
            post.IsAccepted = postVM.IsAccepted;
            post.IsArchived = postVM.IsArchived;            
            if(postVM.SetNewAcceptanceDate == true)
            {
                post.AcceptanceDate = DateTime.Now;
            }
            _context.SaveChanges();
        }

        public PostVM GetPostByID(int id)
        {
            var post = _context.Posts.FirstOrDefault(m => m.PostID == id);
            if (post == null)
            {
                return null;
            }
            var photos = _context.Photos.Where(m => m.PostIDref == id);

            var postVM = new PostVM()
            {
                PostID = id,
                Title = post.Title,
                Txt = post.Txt,
                IsAccepted = post.IsAccepted,
                IsArchived = post.IsArchived,
                CreationDate = post.CreationDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"),
                AcceptanceDate = post.AcceptanceDate,
                UserID = post.UserID,
                UserName = post.UserName,
                Created = post.CreationDate,
                NoComments = GetCommentsNumber(post.PostID),
            };
            postVM.Photo = photos.FirstOrDefault().Uri;
            return postVM;
        }

        public PhotoVM GetPhotoByPostIdRef(int id)
        {
            var photo = _context.Photos.FirstOrDefault(m => m.PostIDref == id);
            if (photo == null)
            {
                return null;
            }
            var photoVM = new PhotoVM()
            {
                PhotoID = photo.PhotoID,
                Name = photo.Name,
                Uri = photo.Uri,
            };
            return photoVM;
        }

        public void Delete(int id)
        {
            var post = _context.Posts.First(m => m.PostID == id);
            var photo = _context.Photos.First(m => m.PostIDref == id);
            _context.Photos.Remove(photo);
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }
        
        public List<PostVM> GetUserContent(string userId)
        {
            IEnumerable<Post> posts;
            posts = _context.Posts.Where(m => m.UserID == userId).OrderByDescending(m => m.CreationDate).ToList();
            var model = new List<PostVM>();
            foreach (var m in posts)
            {
                var post = new PostVM()
                {
                    PostID = m.PostID,
                    Title = m.Title,
                    Txt = m.Txt,
                    IsAccepted = m.IsAccepted,
                    IsArchived = m.IsArchived,
                    CreationDate = m.CreationDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"),
                    Created = m.CreationDate,
                    AcceptanceDate = m.AcceptanceDate,
                };
                var Photo = _context.Photos.Where(m => m.PostIDref == post.PostID);
                post.Photo = Photo.FirstOrDefault().Uri;

                model.Add(post);
            }
            return model;
        }

        public void CreatePost(PostVM postVM, PhotoVM photo)
        {
            
            Post postmodel = new Post()
            {
                Title = postVM.Title,
                Txt = postVM.Txt,
                CreationDate = DateTime.Now,
                IsAccepted = false,
                IsArchived = false,
                UserID = postVM.UserID,
                UserName = postVM.UserName,
                AcceptanceDate = null,
            };

            Photo photomodel = new Photo()
            {
                Name = photo.Name,
                Post = postmodel,
                Uri = photo.Uri,
            };
            _context.Posts.Add(postmodel);
            _context.Photos.Add(photomodel);
            _context.SaveChanges();
        }

        public void AddComment(CommentVM commentVM)
        {
            var comment = new Comment()
            {
                CommentID = commentVM.CommentID,
                UserID = commentVM.UserID,
                UserName = commentVM.UserName,
                Txt = commentVM.Txt,
                CreationDate = DateTime.Now,
                EditDate = null,
                PostIDref = commentVM.PostIDref,
            };
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
        public List<CommentVM> GetCommentsInContent(int postID)
        {
            IEnumerable<Comment> comments;
            comments = _context.Comments.Where(m => m.PostIDref == postID).ToList();
            var model = new List<CommentVM>();
            foreach (var m in comments)
            {
                var comment = new CommentVM()
                {
                    CommentID = m.CommentID,
                    UserID = m.UserID,
                    UserName = m.UserName,
                    Txt = m.Txt,
                    CreationDate = m.CreationDate.ToString("dd MMMM yyyy HH:mm:ss"),
                    EditDate = m.EditDate?.ToString("dd MMMM yyyy HH:mm:ss"),
                    PostIDref = m.PostIDref,
                };
                model.Add(comment);
            }
            return model;
        }

        public List<PostVM> GetAllPostsByAcceptance()
        {
            IEnumerable<Post> posts;
            posts = _context.Posts.OrderByDescending(m => m.AcceptanceDate).ToList();
            var model = new List<PostVM>();

            foreach (var m in posts)
            {
                var post = new PostVM()
                {
                    PostID = m.PostID,
                    UserName = m.UserName,
                    Title = m.Title,
                    Txt = m.Txt,
                    IsAccepted = m.IsAccepted,
                    IsArchived = m.IsArchived,
                    CreationDate = m.CreationDate.ToString("dd MMMM yyyy HH:mm:ss"),
                    AcceptanceDate = m.AcceptanceDate,
                    Created = m.CreationDate,
                    NoComments = GetCommentsNumber(m.PostID),
                };
                var Photo = _context.Photos.Where(m => m.PostIDref == post.PostID);
                post.Photo = Photo.FirstOrDefault().Uri;


                model.Add(post);
            }
            return model;
        }

        public List<PostVM> GetAllPostsByCreation()
        {
            IEnumerable<Post> posts;
            posts = _context.Posts.OrderByDescending(m => m.CreationDate).ToList();
            var model = new List<PostVM>();

            foreach (var m in posts)
            {
                var post = new PostVM()
                {
                    PostID = m.PostID,
                    UserName = m.UserName,
                    Title = m.Title,
                    Txt = m.Txt,
                    IsAccepted = m.IsAccepted,
                    IsArchived = m.IsArchived,
                    CreationDate = m.CreationDate.ToString("dd MMMM yyyy HH:mm:ss"),
                    AcceptanceDate = m.AcceptanceDate,
                    Created = m.CreationDate,
                    NoComments = GetCommentsNumber(m.PostID),
                };
                var Photo = _context.Photos.Where(m => m.PostIDref == post.PostID);
                post.Photo = Photo.FirstOrDefault().Uri;


                model.Add(post);
            }
            return model;
        }

        public int GetCommentsNumber(int postID)
        {
            IEnumerable<Comment> comments;
            comments = _context.Comments.Where(m => m.PostIDref == postID).ToList();
            int x=0;
            foreach (var m in comments)
            {
                x = x + 1;
            }
            return x;
        }
    }
}
