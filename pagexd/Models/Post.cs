﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Models
{
    public class Post
    {
        [Key]
        public int PostID { get; set; }
        [Required]
        public string Title { get; set; }
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string Txt { get; set; }
        public bool IsAccepted { get; set; }
        public DateTime? AcceptanceDate { get; set; }
        public bool IsArchived { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Comment> Comments { get; set; }
        public int PhotoID { get; set; }
        public Photo Photo { get; set; } 
    }
}
