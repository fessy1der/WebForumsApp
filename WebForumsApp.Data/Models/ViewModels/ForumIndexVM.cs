using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class ForumIndexVM
    {
        public IEnumerable<ForumListingVM> ForumsList { get; set; }
    }
}
