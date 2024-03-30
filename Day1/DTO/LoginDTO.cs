using System.ComponentModel.DataAnnotations;

namespace Day1.DTO
{
    public class LoginDTO
    {
        [Required]
        public string userName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
