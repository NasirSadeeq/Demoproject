using System.ComponentModel.DataAnnotations;

namespace Demoproject.Models
{
    public class Admin
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string MobileNo { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string roles { get; set; }
    }
}
