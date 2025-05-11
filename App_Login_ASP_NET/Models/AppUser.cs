using AspNetCore.Identity.MongoDbCore.Models;

using MongoDbGenericRepository.Attributes;

namespace App_Login_ASP_NET.Models
{

    [CollectionName("User")]
    public class AppUser : MongoIdentityUser // Classe padronizada utilizada para cadastrar usuários.
    {



    }

}