using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class AddForumVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        public IFormFile ImageUpload { get; set; }
    }
}
