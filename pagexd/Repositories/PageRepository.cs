﻿using pagexd.Data;
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

        
        public void Edit(PostVM postVM)
        {
            var post = _context.Posts.First(m => m.PostID == postVM.PostID);
            post = new Post()
            {
                PostID = postVM.PostID,
                Title = postVM.Title,
                Txt = postVM.Txt,
                CreationDate = postVM.CreationDate,
                IsAccepted = postVM.IsAccepted,
                IsArchived = postVM.IsArchived,
                UserID = postVM.UserID,
            };
            _context.Update(post);
            _context.SaveChanges();
        }
        public void EditPostModel(Post post,string title, string txt, bool isaccepted, bool isarchivized)
        {
            var postmodel = new Post()
            {
                PostID = post.PostID,
                Title = title,
                Txt = txt,
                UserID = post.UserID,
                CreationDate = post.CreationDate,
                IsAccepted = isaccepted,
                IsArchived = isarchivized,
            };
            post = postmodel;
            _context.SaveChanges();
        }
        public Post GetPostModelByID(int id)
        {
            var post = _context.Posts.First(m => m.PostID == id);
            return post;
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
                CreationDate = DateTime.Now,
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

    }
}