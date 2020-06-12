using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class SearchResultVM
    {
        public IEnumerable<PostListingVM> Posts { get; set; }
        public string SearchQuery { get; set; }
        public bool EmptySearchResults { get; set; }
    }
}
