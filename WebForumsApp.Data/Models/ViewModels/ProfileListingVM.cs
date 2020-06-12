using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class ProfileListingVM
    {
        public IEnumerable<ProfileVM> Profiles { get; set; }
    }
}
