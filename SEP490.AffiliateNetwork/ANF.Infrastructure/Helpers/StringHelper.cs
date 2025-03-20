using System.Security.Cryptography;
using System.Text;

namespace ANF.Infrastructure.Helpers
{
    public static class StringHelper
    {
        public static string GenerateUniqueCode()
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                return BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 10);
            }
        }
    }
}
