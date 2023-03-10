using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using VaultSave.Saver;
using VaultSave.Systems;

namespace VaultSave.Commands
{
    public class VSSaveJsonToAesCommand:ISaver
    {
        public void Execute<T>(T dataToSave, string path, VSSystemData vsSystemData)
        {
            string dataAsJson = JsonConvert.SerializeObject(dataToSave);
            byte[] encryptedData = EncryptStringToBytesAes(dataAsJson, vsSystemData.Password);
            File.WriteAllBytes(path, encryptedData);
        }
        private static byte[] EncryptStringToBytesAes(string plainText, string password)
        {
            byte[] encrypted;
            byte[] salt = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (Aes aes = Aes.Create())
            {
                Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(password, salt);
                aes.Key = deriveBytes.GetBytes(aes.KeySize / 8);
                aes.IV = deriveBytes.GetBytes(aes.BlockSize / 8);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
    }
}