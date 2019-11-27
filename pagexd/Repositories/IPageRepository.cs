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
        List<PostVM> GetAllPosts();
        PostVM GetPostByID(int id);

        Post GetPostModelByID(int id);
        void EditPostModel(Post post, string title, string txt, bool isaccepted, bool isarchivized);
        void Edit(PostVM postVM);
        void Delete(int id);

    }
}
