namespace Zanshin.Domain.Services
{
    using System.Security.Cryptography;
    using System.Text;

    using Zanshin.Domain.Exceptions;
    using Zanshin.Domain.Extensions;

    public sealed class HashService
    {
        /// <summary>
        /// Creates the sh a256 hash.
        /// </summary>
        /// <param name="stringToHash">The string to hash.</param>
        /// <returns></returns>
        public static string CreateSha256Hash(string stringToHash)
        {
            return stringToHash.Sha256Hash();
        }

        /// <summary>
        ///   Creates the hash string.
        /// </summary>
        /// <param name="stringToHash"> The string to hash. </param>
        /// <returns> </returns>
        public static string HashString(string stringToHash)
        {
            if (string.IsNullOrEmpty(stringToHash))
            {
                throw new ParameterNullException("stringToHash");
            }

            return ByteArrayToString(CreateHash(stringToHash));
        }

        /// <summary>
        ///   Creates the hash.
        /// </summary>
        /// <param name="stringToHash"> The string to hash. </param>
        /// <returns> </returns>
        /// <remarks>Uses the SHA1Managed hash</remarks>
        internal static byte[] CreateHash(string stringToHash)
        {
            if (string.IsNullOrEmpty(stringToHash))
            {
                throw new ParameterNullException("stringToHash");
            }
            byte[] uriArray = Encoding.UTF8.GetBytes(stringToHash);
            var hash = new SHA1Managed().ComputeHash(uriArray);
            uriArray = null;
            return hash;
        }

        /// <summary>
        ///   Bytes the array to string.
        /// </summary>
        /// <param name="arrInput"> The arr input. </param>
        /// <returns> </returns>
        internal static string ByteArrayToString(byte[] arrInput)
        {
            if (arrInput == null)
            {
                throw new ParameterNullException("arrInput");
            }

            int i;
            StringBuilder sb = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sb.Append(arrInput[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}