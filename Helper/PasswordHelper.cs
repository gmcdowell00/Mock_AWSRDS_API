using Microsoft.IdentityModel.Tokens;
using Mock_AWSRDS_API.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Mock_AWSRDS_API.Helper
{
    public class PasswordHelper : IPasswordHelper
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        public byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(32); // 256-bit hash
            }
        }

        public  bool ValidatePassword(string password, string saltString, string hashedPasswordString)
        {
            byte[] salt = Convert.FromBase64String(saltString);
            byte[] hashedPassword = Convert.FromBase64String(hashedPasswordString);

            byte[] newHashedPassword = HashPassword(password, salt);

            // Compare the new hashed password with the stored hashed password
            return AreByteArraysEqual(hashedPassword, newHashedPassword);
        }

        private static bool AreByteArraysEqual(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public string RandomString(int length)
        {            
           
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int randomIndex = new Random().Next(chars.Length);
                char randomChar = chars[randomIndex];
                stringBuilder.Append(randomChar);
            }

            return stringBuilder.ToString();
        }

        public string GenerateJwtToken(string secretKey, string issuer, string audience, string username)
        {
            // Create claims for the token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, $"{username}"),
                new Claim(ClaimTypes.Email, $"{username}.com"),
                // Add more claims as needed
            };

            // Create the signing credentials
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Set token expiration
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };

            // Create the token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Generate the JWT token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Write the token as a string
            return tokenHandler.WriteToken(token);
        }
    }
}
