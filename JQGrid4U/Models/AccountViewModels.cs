using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace JQGrid4U.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
       // [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


	public class HashViewModel
	{

		[Required]
		[Remote("IsValidEmailAdd", "Account", ErrorMessage = "Invalid Email Address")]
		[Display(Name = "Email Adress")]
		public string EmailAdd { get; set; }

		//[Required]
		//[Display(Name = "Old Password")]
		[Required(ErrorMessage = "Old Password Is Required")]
		[DataType(DataType.Password)]
		public string OldPwd { get; set; }


		//[Required]
		//[Display(Name = "New Password")]
		[Required(ErrorMessage = "New Password Is Required")]
		[DataType(DataType.Password)]
		public string NewPwd { get; set; }

		//[Required]
		//[Display(Name = "Confirm Password")]
		[Required(ErrorMessage = "Confirmed Password Is Required")]
		[DataType(DataType.Password)]
		public string ConfirmPwd { get; set; }


	}
	public class ParameterViewModel
	{

		[Required]
		[Display(Name = "Temperature Critical Level")]
		public string TempCriticalLevel { get; set; }

		[Required]
		[Display(Name = "Pulley Device Expiry")]
		public string PulleyDeviceExpiry { get; set; }

		[Required]
		[Display(Name = "Roller Device Expiry")]
		public string RollerDeviceExpiry { get; set; }

		[Required]
		[Display(Name = "Email Alert Interval")]
		public string EmailAlertInterval { get; set; }


		[Required]
		[Display(Name = "Communication Port")]
		public string ComPort { get; set; }



	}
	public class LoginViewModel
    {
        [Required]
        [Remote("IsUserNameExist", "Account", ErrorMessage = "User ID Not Found")]
        [Display(Name = "Email address")]
        public string UserName { get; set; }


		[Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

	public class AddNewUser
	{
		[Required]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }
	}
}
