using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class DeletePostVM
    {
        public int PostId { get; set; }
        public string PostAuthor { get; set; }
        public string PostContent { get; set; }
    }
}
