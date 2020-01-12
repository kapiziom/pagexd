using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.ViewModels
{
    public class PhotoVM
    {
        public int PhotoID { get; set; }
        public string PhotoPath { get; set; }
        public string Name { get; set; }
        [Required]
        public IFormFile File { get; set; }
        public int PostID { get; set; }
        public string Uri { get; set; }
        public string PathForView { get; set; }
    }
}
