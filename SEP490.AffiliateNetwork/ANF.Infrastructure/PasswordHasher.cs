using System.Security.Cryptography;
using System.Text;

namespace ANF.Infrastructure
{
    public class PasswordHasher
    {
        // Generates a hashed password with salting
        public static string HashPassword(string plainPassword)
        {
            // Generate a 16-byte random salt
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);

            // Combine the password and salt, then hash
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(plainPassword + salt);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Combine salt and hash into one string for storage
                string saltedHash = $"{salt}.{Convert.ToBase64String(hashBytes)}";
                return saltedHash;
            }
        }

        // Verifies the inputted password by comparing it with the stored hash
        public static bool VerifyPassword(string inputtedPassword, string storedPasswordHashed)
        {
            // Split the stored hashed password into salt and hash
            var parts = storedPasswordHashed.Split('.');
            if (parts.Length != 2)
            {
                throw new FormatException("Stored password format is invalid.");
            }

            string storedSalt = parts[0];
            string storedHash = parts[1];

            // Re-hash the inputted password using the stored salt
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(inputtedPassword + storedSalt);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Compare the newly computed hash with the stored hash
                string inputtedHash = Convert.ToBase64String(hashBytes);
                return storedHash == inputtedHash;
            }
        }
    }
}
