using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Models
{
    public class Photo
    {
        [Key]
        public int PhotoID { get; set; }
        public string PhotoPath { get; set; }
        public string Name { get; set; }
        public int PostIDref { get; set; }
        public string PathForView { get; set; }
        public Post Post { get; set; }
    }
}
