namespace Security.Server.Hashing
{
    using System;
    using System.Linq;
    using System.Text;

    public class Argon2i
    {
        private readonly int memLimit;
        private readonly int opsLimit;
        private readonly int algorithm;
        private readonly uint bufferSize;

        public Argon2i(int memLimit, int opsLimit)
        {
            this.memLimit = memLimit;
            this.opsLimit = opsLimit;
            algorithm = Libsodium.crypto_pwhash_alg_argon2i13();
            bufferSize = Libsodium.crypto_pwhash_strbytes();
        }

        public bool Verify(string password, string hashedPassword)
        {
            var result = Libsodium.crypto_pwhash_str_verify(hashedPassword, password, password.Length);
            return result == 0;
        }

        public string HashPassword(string password)
        {
            var buffer = new byte[bufferSize];
            var result = Libsodium.crypto_pwhash_str_alg(
                buffer, password, password.Length,
                opsLimit, memLimit, algorithm);

            if (result != 0)
            {
                throw new Exception("An unexpected error has occurred.");
            }

            return Encoding.UTF8.GetString(buffer.Where(b => b != 0).ToArray());
        }

        public string HashPassword(string password, string salt)
        {
            var passwordBytes = GetBytes(password);
            var saltBytes = GetBytes(salt);
            passwordBytes = passwordBytes.Concat(saltBytes).ToArray();
            var buffer = new byte[bufferSize];
            var result = Libsodium.crypto_pwhash(
                buffer, bufferSize,
                passwordBytes, password.Length,
                saltBytes,
                opsLimit, memLimit, algorithm);

            if (result != 0)
            {
                throw new Exception("An unexpected error has occurred.");
            }

            return Encoding.UTF8.GetString(buffer.Where(b => b != 0).ToArray());
        }

        private byte[] GetBytes(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }
    }
}
