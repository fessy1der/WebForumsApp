using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class ForumListingVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int NumberOfPosts { get; set; }
        public int NumberOfUsers { get; set; }
        public string Image { get; set; }
        public bool HasRecentPost { get; set; }

        public PostListingVM Latest { get; set; }
        public IEnumerable<PostListingVM> AllPosts { get; set; }
    }
}
