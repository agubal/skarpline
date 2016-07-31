using System.ComponentModel.DataAnnotations;

namespace Skarpline.Entities.Models.Identity
{
    /// <summary>
    /// View model for registration object
    /// </summary>
    public class UserRegisterModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
