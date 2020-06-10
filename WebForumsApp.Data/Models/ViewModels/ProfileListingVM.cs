using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    class ProfileListingVM
    {
        public IEnumerable<ProfileVM> Profiles { get; set; }
    }
}
