using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Web.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(controller:"Account", action: "CheckIsEmailTaken")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password", ErrorMessage ="Password and confirmation does not match.")]
        public string ConfirmPassword { get; set; }
    }
}
