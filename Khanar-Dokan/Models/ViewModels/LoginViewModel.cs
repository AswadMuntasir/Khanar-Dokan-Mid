using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Khanar_Dokan.Models.DataAccess;

namespace Khanar_Dokan.Models.ViewModels
{
    public class LoginViewModel
    {
        [Key]
        [Required]
        [Display(Name = "email")]
        public string uemail { get; set; }

        [Required]
        [Display(Name = "password")]
        [DataType(DataType.Password)]
        public string upassword { get; set; }
    }
}