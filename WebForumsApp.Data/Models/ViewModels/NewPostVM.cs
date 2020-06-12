using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class NewPostVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ForumName { get; set; }
        public string ForumImageUrl { get; set; }
        public int ForumId { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }
        public string AuthorName { get; set; }
    }
}
