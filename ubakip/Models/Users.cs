using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ubakip.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,6})+)$")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Login")]
        [RegularExpression(@"^[a-zA-Z0-9]{5,15}$")]
        public string Login { get; set; }

        public string Photo { get; set; }

        public int Role { get; set; }

        public string Theme { get; set; }

        public string Lang { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]{1,15}$")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9]{1,15}$")]
        public string LastName { get; set; }
    }
}