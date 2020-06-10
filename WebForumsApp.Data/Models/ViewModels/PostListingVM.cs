﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class PostListingVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int AuthorRating { get; set; }
        public string AuthorId { get; set; }
        public string DatePosted { get; set; }
        public ForumListingVM Forum { get; set; }
        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public string ForumImageUrl { get; set; }
        public int NumberOfReplies { get; set; }
    }
}
