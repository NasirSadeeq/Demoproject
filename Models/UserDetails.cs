using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Demoproject.Models
{
    public class UserDetails
    {
      
       public int Id { get; set; }
        [Required]
       public string FisrtName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public int MobileNo { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string roles { get; set; }

    }
}
