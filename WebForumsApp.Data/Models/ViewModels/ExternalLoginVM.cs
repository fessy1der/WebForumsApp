using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebForumsApp.Data.Models.ViewModels
{
    public class ExternalLoginVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
