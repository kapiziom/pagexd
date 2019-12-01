using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.ViewModels
{
    public class PostWithCommentsVM
    {
        public PostVM PostVM { get; set; }
        public CommentVM CommentVM { get; set; }
        public List<CommentVM> CommentVMs { get; set; }
    }
}
