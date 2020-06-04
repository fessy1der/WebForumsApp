using System;

namespace WebForumsApp.Data
{
    public class Reply
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
    }
}