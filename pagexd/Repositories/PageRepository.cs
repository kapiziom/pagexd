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

        public PageRepository(ModelDbContext modelDbContext)
        {
            _context = modelDbContext;
        }
 
        public void Edit(PostVM postVM, int id)
        {
            var post = _context.Posts.FirstOrDefault(m => m.PostID == id);
            post.Title = postVM.Title;
            post.Txt = postVM.Txt;
            post.IsAccepted = postVM.IsAccepted;
            post.IsArchived = postVM.IsArchived;
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
                CreationDate = post.CreationDate,
                UserID = post.UserID,


            };
            postVM.Photo = photos.FirstOrDefault().PathForView;
            return postVM;
        }

        public void Delete(int id)
        {
            var post = _context.Posts.First(m => m.PostID == id);
            var photo = _context.Photos.First(m => m.PostIDref == id);
            File.Delete(photo.PhotoPath);
            _context.Photos.Remove(photo);
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }


        public List<PostVM> GetAllPosts()
        {
            IEnumerable<Post> posts;
            posts = _context.Posts.ToList();
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
                    CreationDate = m.CreationDate,
                };
                var Photo = _context.Photos.Where(m => m.PostIDref == post.PostID);
                post.Photo = Photo.FirstOrDefault().PathForView;
                

                model.Add(post);
            }
            return model;
        }

        public List<PostVM> GetUserContent(string userId)
        {
            IEnumerable<Post> posts;
            posts = _context.Posts.Where(m => m.UserID == userId).ToList();
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
                    CreationDate = m.CreationDate,
                };
                var Photo = _context.Photos.Where(m => m.PostIDref == post.PostID);
                post.Photo = Photo.FirstOrDefault().PathForView;

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
                CreationDate = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"),
                IsAccepted = postVM.IsAccepted,
                IsArchived = postVM.IsArchived,
                UserID = postVM.UserID,
            };

            Photo photomodel = new Photo()
            {
                PhotoPath = photo.PhotoPath,
                PathForView = photo.PathForView,
                Name = photo.Name,
                Post = postmodel,
            };
            _context.Posts.Add(postmodel);
            _context.Photos.Add(photomodel);
            _context.SaveChanges();
        }

        public void AddComment(CommentVM commentVM, int id)
        {
            //var post = _context.Posts.FirstOrDefault(m => m.PostID == id);
            //commentVM.PostIDref = id/*post.PostID*/;
            var comment = new Comment()
            {
                CommentID = commentVM.CommentID,
                UserID = commentVM.UserID,
                Txt = commentVM.Txt,
                CreationDate = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"),
                EditDate = null,
                PostIDref = commentVM.PostIDref,
                //Post = post,
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
                    Txt = m.Txt,
                    CreationDate = m.CreationDate,
                    EditDate = m.EditDate,
                    PostIDref = m.PostIDref,
                };
                model.Add(comment);
            }
            return model;
        }

    }
}
