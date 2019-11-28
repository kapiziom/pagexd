﻿using System;
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
        public string UserID { get; set; }
        [Required]
        public string Txt { get; set; }
        public string CreationDate { get; set; }
        public string EditDate { get; set; }
        public int PostIDref { get; set; }
        public Post Post { get; set; }
    }
}
