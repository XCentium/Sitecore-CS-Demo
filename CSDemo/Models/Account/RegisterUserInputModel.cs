using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CSDemo.Models.Account
{
    public class RegisterUserInputModel
    {
		[Required]
        [DataType(DataType.Password), Display(Name = "Confirm password"), Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string ExternalId { get; set; }

		[Required]
		[StringLength(0x19, ErrorMessage = "The {0} must be maximum {1} characters long."), Display(Name = "First Name")]
        public string FirstName { get; set; }

		[Required]
		[StringLength(0x19, ErrorMessage = "The {0} must be maximum {1} characters long."), Display(Name = "Last Name")]
        public string LastName { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail address of existing customer"), Display(Name = "Email Of Existing Customer")]
        public string LinkupEmail { get; set; }

		[Required, StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6), DataType(DataType.Password), Display(Name = "Password")]
        public string Password { get; set; }

        public string SignupSelection { get; set; }

		[Required, Display(Name = "Email")]
        public string UserName { get; set; }
    }
}