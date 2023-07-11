using System.Security.Cryptography;
using System.Text;

namespace StudentAttendanceApiBLL
{
    public static class EncryptionUtility
    {
        public static string GetHashPassword(string password)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                builder.Append(hashValue[i].ToString("x2"));
            }
            return builder.ToString();
        }

        //public static string GetHash(string text)
        //{
        //    var hashGenerator = new HashGenerator(Constants.encriptionUserPassKey);
        //    return hashGenerator.GenerateHash(text);
        //}

    }
}