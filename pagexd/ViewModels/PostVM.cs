using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.ViewModels
{
    public class PostVM
    {
        public int PostID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Title { get; set; }
        public string Txt { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public bool SetNewAcceptanceDate { get; set; }
        public bool IsArchived { get; set; }
        public string CreationDate { get; set; }
        public DateTime Created { get; set; }
        public string Photo { get; set; }
        public int NoComments { get; set; }
    }   
}
