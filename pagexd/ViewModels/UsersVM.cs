using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pagexd.ViewModels
{
    public class UsersVM
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public Guid UserRoleId { get; set; }
    }
}
