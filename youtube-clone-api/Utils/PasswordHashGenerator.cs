using System.Security.Cryptography;

namespace youtube_clone_api.Utils
{
    public class PasswordHashGenerator
    {
        private readonly string _password;
        public PasswordHashGenerator(string password)
        {
            _password = password;
        }

        public void CreatePasswordHash(out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(_password));
            }
        }

        public bool VerifyPasswordHash(byte[] userPasswordHash, byte[] userPasswordSalt)
        {
            using (var hmac = new HMACSHA512(userPasswordSalt))
            {
                var requestPasswordHashed = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(_password));
                return requestPasswordHashed.SequenceEqual(userPasswordHash);
            }
        }
    }
}
