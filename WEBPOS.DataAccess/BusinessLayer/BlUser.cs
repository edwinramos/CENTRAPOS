using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;

namespace WEBPOS.DataAccess.BusinessLayer
{
    public static class BlUser
    {
        public static IEnumerable<DeUser> ReadAll()
        {
            var dl = new DlUser();
            return dl.ReadAll();
        }
        public static IQueryable<DeUser> ReadAllQueryable()
        {
            var dl = new DlUser();
            return dl.ReadAllQueryable();
        }
        public static IEnumerable<DeUser> Read(DeUser obj)
        {
            var dl = new DlUser();
            return dl.Read(obj);
        }

        public static void Save(DeUser obj)
        {
            var dl = new DlUser();
            obj.Password = EncryptString(obj.Password, obj.UserCode);
            dl.Save(obj);
        }

        public static void Delete(DeUser obj)
        {
            var dl = new DlUser();
            dl.Delete(obj.UserCode);
        }

        public static string GetNextUserCode()
        {
            var count = ReadAll().Count() + 1;
            string str = count.ToString().PadLeft(7, '0');

            while (ReadAllQueryable().Any(x => x.UserCode == str))
            {
                count++;
                str = count.ToString().PadRight(7, '0');
            }

            return str;
        }

        private const int keysize = 256;
        private const string initVector = "pemgail9uzpgzl88";
        public static string EncryptString(string passwordText, string userName)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(passwordText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(userName, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }
        //Decrypt
        public static string DecryptString(string cipherText, string userName)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(userName, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}
