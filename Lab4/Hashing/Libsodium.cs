namespace Security.Server.Hashing
{
    using System.Runtime.InteropServices;

    public class Libsodium
    {
        private const string Name = "libsodium";

        static Libsodium()
        {
            sodium_init();
        }

        [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void sodium_init();

        [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void randombytes_buf(byte[] buffer, int size);

        [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int crypto_pwhash(
            byte[] buffer, long bufferLen,
            byte[] password, long passwordLen,
            byte[] salt,
            long opsLimit, int memLimit, int alg);

        [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int crypto_pwhash_str_alg(
            byte[] buffer, string password, long passwordLength,
            long opsLimit, int memLimit, int alg);

        [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int
            crypto_pwhash_str_verify(string hashedPassword, string password, long passwordLength);

        [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint crypto_pwhash_strbytes();

        [DllImport(Name, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int crypto_pwhash_alg_argon2i13();
    }
}
