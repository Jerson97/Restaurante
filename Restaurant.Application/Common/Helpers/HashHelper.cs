using System.Security.Cryptography;
using System.Text;

namespace Restaurant.Application.Common.Helpers
{
    public static class HashHelper
    {
        public static string HashPin(string pin)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(pin);
            byte[] hash = sha256.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString().ToUpper();
        }
    }
}
