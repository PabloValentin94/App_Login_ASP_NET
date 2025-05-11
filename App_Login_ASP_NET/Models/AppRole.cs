using AspNetCore.Identity.MongoDbCore.Models;

using MongoDbGenericRepository.Attributes;

namespace App_Login_ASP_NET.Models
{

    [CollectionName("Role")]
    public class AppRole : MongoIdentityRole // Classe padronizada utilizada para cadastrar cargos.
    {



    }

}