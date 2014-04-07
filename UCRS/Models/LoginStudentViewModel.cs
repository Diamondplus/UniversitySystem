using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UCRS.Models
{
    public class LoginStudentViewModel
    {
        //[Required(ErrorMessage = "The email address is required")]
        //[Display(Name = "Student Email")]
        //[EmailAddress(ErrorMessage = "Invalid Email Address")]

        [Required(ErrorMessage = "Please enter the email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Student Email")]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password is required !")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "The Password must be at least {2} characters long.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public LoginStudentViewModel()
        {
            this.Email = string.Empty;
            this.Password = string.Empty;
        }

        public LoginStudentViewModel(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
}