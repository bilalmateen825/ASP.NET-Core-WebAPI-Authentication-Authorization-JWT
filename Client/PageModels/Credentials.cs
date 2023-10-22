using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Client.PageModels
{
    public class Credential
    {
        [Required]
        [DisplayName("User Name")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
