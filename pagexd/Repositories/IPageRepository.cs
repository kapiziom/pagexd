using pagexd.Models;
using pagexd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Repositories
{
    public interface IPageRepository
    {
        List<PostVM> GetUserContent(string userID);
        void CreatePost(PostVM postVM, PhotoVM photo);
        List<PostVM> GetAllPostsByCreation();
        List<PostVM> GetAllPostsByAcceptance();
        PostVM GetPostByID(int id);
        void Edit(PostVM postVM, int id);
        void AdminEdit(PostVM postVM, int id);
        void Delete(int id);
        void AddComment(CommentVM comment);
        List<CommentVM> GetCommentsInContent(int postID);
        int GetCommentsNumber(int postID);
    }
}
