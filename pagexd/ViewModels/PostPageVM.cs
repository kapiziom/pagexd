using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.ViewModels
{
    public class PostPageVM
    {
        public List<PostVM> postVMs { get; set; }
        public int PageCount { get; set; }
    }
}
