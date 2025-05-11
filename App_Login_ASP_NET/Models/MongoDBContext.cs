using MongoDB.Driver;

namespace App_Login_ASP_NET.Models
{

    public class MongoDBContext
    {

        // Atributos de conexão com o banco de dados.

        public static string? Connection_String { get; set; }

        public static string? Database_Name { get; set; }

        public static bool Is_Ssl { get; set; }

        // Atributo de acesso ao banco de dados.

        private IMongoDatabase? Database { get; } // Somente leitura.

        // Método construtor da classe.

        public MongoDBContext()
        {

            try
            {

                MongoClientSettings Connection_Settings = MongoClientSettings.FromUrl(new MongoUrl(Connection_String));

                if (Is_Ssl)
                {

                    Connection_Settings.SslSettings = new SslSettings()
                    {

                        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12

                    };

                }

                MongoClient Connection = new MongoClient(Connection_Settings);

                this.Database = Connection.GetDatabase(Database_Name);

            }

            catch (Exception ex)
            {

                throw new Exception("Não foi possível se conectar ao MongoDB.", ex);

            }

        }

        // Coleções.

        public IMongoCollection<User> Users { get { return Database.GetCollection<User>("User"); } }

        public IMongoCollection<Role> Roles { get { return Database.GetCollection<Role>("Role"); } }

    }

}