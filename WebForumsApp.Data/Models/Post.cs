using System;
using System.Collections.Generic;

namespace WebForumsApp.Data
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Forum Forum { get; set; }
        public virtual IEnumerable<Reply> Replies{ get; set; }
    }
}