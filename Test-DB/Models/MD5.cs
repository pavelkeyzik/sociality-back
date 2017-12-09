using System.Security.Cryptography;
using System.Text;

namespace Test_DB.Models
{
    public class MD5
    {
        private readonly string password;

        public MD5(string password)
        {
            this.password = password;
        }

        public string getHash()
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(this.password));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}