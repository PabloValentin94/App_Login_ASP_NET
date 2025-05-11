using System.ComponentModel.DataAnnotations;

namespace App_Login_ASP_NET.Models
{

    public class Role
    {

        [Required]
        public string? Nome { get; set; }

    }

}