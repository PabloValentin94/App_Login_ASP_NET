using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

using System.Globalization;

using System.Text;

namespace App_Login_ASP_NET.Models
{

    public class User
    {

        private Guid? _id;

        public Guid? Id { get { return this._id; } set { this._id = value; } }

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

        // Aprofundamento: https://gist.github.com/PabloValentin94/67343c258863eb1d157b881bf5adb074

        public static string Remove_Accents(string user_name)
        {

            string raw_text = user_name.Normalize(NormalizationForm.FormD);

            StringBuilder formatted_string = new StringBuilder();

            foreach (char character in raw_text)
            {

                if (CharUnicodeInfo.GetUnicodeCategory(character) != UnicodeCategory.NonSpacingMark)
                {

                    formatted_string.Append(character);

                }

            }

            return formatted_string.ToString().Normalize(NormalizationForm.FormC);

        }

    }

}