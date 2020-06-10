using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int Rating { get; set; }
        public string UserImage { get; set; }
        public DateTime JoinedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
