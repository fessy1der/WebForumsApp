using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class ExternalLoginsVM
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        public IList<AuthenticationScheme> OtherLogins { get; set; }

        public bool ShowRemoveButton { get; set; }

        public string StatusMessage { get; set; }
    }
}
