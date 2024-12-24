using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LoginLogout.Entities
{
    [Index(nameof(Email),IsUnique =true)]
    [Index(nameof(UserName),IsUnique =true)]
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="First Name is Required")]
        [MaxLength(50,ErrorMessage ="Max 50 characters allowed")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is Required")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [MaxLength(100, ErrorMessage = "Max 50 characters allowed")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User Name is Required")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [MaxLength(64, ErrorMessage = "Max 50 characters allowed")]
        public string Password { get; set; }

        public int Role { get; set; }

        public DateTime? LastLoginDatetime { get; set; }
    }
}
