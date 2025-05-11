using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace App_Login_ASP_NET.Models
{

    public class User
    {

        [Required]
        public string? Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Todo usuário precisa ter um e-mail associado a ele.")]
        public string? Email { get; set; }

        [Required]
        [Phone]
        public string? Phone { get; set; }

        [Required]
        public string? Password { get; set; }

    }

}