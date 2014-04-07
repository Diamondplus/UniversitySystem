using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UCRS.Model
{
    public class Student
    {
            [Key]
            [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
            [Display(AutoGenerateField = true)]
            public int StudentId { get; set; }

            [Required]
            [StringLength(50)]
            public string Email { get; set; }

            [Required]
            [StringLength(100)]
            public string Password { get; set; }

            [Required]
            [StringLength(100)]
            public string PasswordSalt { get; set; }
    }
}