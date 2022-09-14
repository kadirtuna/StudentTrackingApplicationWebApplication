using System.ComponentModel.DataAnnotations;

namespace StudentTrackingApplicationReal.Shared.Models
{
    public class SchoolUser
    {
        [Key]
        [StringLength(30)]
        [MinLength(3)]
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        [StringLength(30)]
        public string UserRole { get; set; }
    }
}
