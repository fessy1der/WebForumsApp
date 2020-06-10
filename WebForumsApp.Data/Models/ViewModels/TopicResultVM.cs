using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class TopicResultVM
    {
        public ForumListingVM Forum { get; set; }
        public IEnumerable<PostListingVM> Posts { get; set; }
        public string SearchQuery { get; set; }
        public bool EmptySearchResults { get; set; }
    }
}
