using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class ApplicationUserVM
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UserImage { get; set; }
        public int Rating { get; set; }
        public bool IsActive { get; set; }
    }
}
