using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Test_DB.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer";
        public const string AUDIENCE = "http://localhost:5000/";
        const string KEY = "BYl2m7UfT32bdPC1UoZVQFypRz3m461G";
        public const int LIFETIME = 25;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}