using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class DeleteForumVM
    {
        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public int UserId { get; set; }
    }
}
