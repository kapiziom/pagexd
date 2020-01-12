using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.ViewModels
{
    public class CommentVM
    {
        public int CommentID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        [Required]
        [StringLength(600, ErrorMessage = "The Comment must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Add comment")]
        public string Txt { get; set; }
        public string CreationDate { get; set; }
        public string EditDate { get; set; }
        public int PostIDref { get; set; }
    }
}
