using System.ComponentModel.DataAnnotations;

namespace LoginLogout.Entities
{
    public class UserInformation
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "Address Line 1 is Required")]
        [MaxLength(500,ErrorMessage ="Maximum 500 characters is allowed")]
        public string AddressLine1 { get; set; }


        [MaxLength(500, ErrorMessage = "Maximum 500 characters is allowed")]
        public string? AddressLine2 { get; set; }

        [Required(ErrorMessage = "Pincode is Required")]
        [MaxLength(6, ErrorMessage = "Maximum 6 digits is allowed")]
        public string Pincode { get; set; }

        [Required(ErrorMessage = "Phone Number is Required")]
        [MaxLength(10, ErrorMessage = "Maximum 10 digits is allowed")]
        public string PhoneNumber { get; set; }
    }
}