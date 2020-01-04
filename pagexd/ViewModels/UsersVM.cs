using System;
using System.Collections.Generic;

namespace pagexd.ViewModels
{
    public class UsersVM
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserRole { get; set; }
        public Guid UserRoleId { get; set; }
        public string AccInfo { get; set; }

        public List<UserRoleCheckVM> Roles { get; set; }
    }
}
