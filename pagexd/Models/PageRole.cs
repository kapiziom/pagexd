using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.Models
{
    public class PageRole : IdentityRole<Guid>
    {
        public string Description { get; set; }
    }
}
