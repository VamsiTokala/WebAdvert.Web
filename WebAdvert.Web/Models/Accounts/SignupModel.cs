using System.ComponentModel.DataAnnotations;

namespace WebAdvert.Web.Models.Accounts
{
    public class SignupModel
    {
        [Required] //Specifies that a data field value is required.
        [EmailAddress]// checks email format
        [Display(Name ="Email")]
        public string Email { get; set; } //prop tab it will add a public property

        [Required]
        [DataType(DataType.Password)] // for password type we need to use this
        [StringLength(6, ErrorMessage = "Password must be at least six checrectors long!")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)] // for password type we need to use this
        [Compare("Password", ErrorMessage ="Password and its confirmation do not match")]
        [Display(Name = "ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
