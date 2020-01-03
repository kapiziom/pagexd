using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace pagexd.Models
{
    public class PageUser : IdentityUser<Guid>
    {
        public string AccInfo { get; set; }
    }
}
