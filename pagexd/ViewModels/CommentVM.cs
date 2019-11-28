using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.ViewModels
{
    public class CommentVM
    {
        public int CommentID { get; set; }
        public string UserID { get; set; }
        public string Txt { get; set; }
        public string CreationDate { get; set; }
        public string EditDate { get; set; }
        public int PostIDref { get; set; }
    }
}
