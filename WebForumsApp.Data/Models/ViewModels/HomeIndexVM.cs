using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class HomeIndexVM
    {
        public string SearchQuery { get; set; }
        public IEnumerable<PostListingVM> LatestPosts { get; set; }
    }
}
