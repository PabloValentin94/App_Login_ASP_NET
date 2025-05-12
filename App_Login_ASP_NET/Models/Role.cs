using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace App_Login_ASP_NET.Models
{

    public class Role
    {

        private Guid _id;

        public Guid Id { get { return this._id; } set { this._id = value; } }

        [Required]
        public string? Nome { get; set; }

    }

}