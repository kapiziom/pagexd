using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public PageUser PageUser { get; set; }
        [Required]
        public string Txt { get; set; }
        public Post Post { get; set; }
    }
}
