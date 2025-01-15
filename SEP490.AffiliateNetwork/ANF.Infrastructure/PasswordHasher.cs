using System.Security.Cryptography;
using System.Text;

namespace ANF.Infrastructure
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords.
    /// </summary>
    public static class PasswordHasher
    {
        /// <summary>
        /// Hashes the specified password using SHA-256 and a random salt.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A string containing the salt and the hashed password, separated by a period.</returns>
        public static string HashPassword(string password)
        {
            // Generate a 16-byte random salt
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator.Fill(saltBytes);
            string salt = Convert.ToBase64String(saltBytes);

            // Combine the password and salt, then hash
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // Combine salt and hash into one string for storage
                string saltedHash = $"{salt}.{Convert.ToBase64String(hashBytes)}";
                return saltedHash;
            }
        }

        /// <summary>
        /// Verifies the specified password against the stored hashed password.
        /// </summary>
        /// <param name="inputtedPassword">The password to verify.</param>
        /// <param name="storedPasswordHashed">The stored hashed password, including the salt.</param>
        /// <returns><c>true</c> if the password is correct; otherwise, <c>false</c>.</returns>
        /// <exception cref="FormatException">Thrown when the stored password format is invalid.</exception>
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
