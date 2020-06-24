using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.Entities.Identity
{
    public class User : IdentityUser
    {
        public const string Administrator = "Admin";
        public const string DefaultAdminPassword = "AdminPassword";
    }
}
