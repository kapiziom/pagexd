using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.ViewModels
{
    public class PostVM
    {
        public int PostID { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public string Txt { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsArchived { get; set; }
        public string CreationDate { get; set; }
        public string Photo { get; set; }
    }   
}
