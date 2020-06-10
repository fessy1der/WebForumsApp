using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class ProfileVM
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string UserRating { get; set; }
        public string UserImage { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public DateTime JoinedOn { get; set; }
        public IFormFile ImageUpload { get; set; }
    }
}
