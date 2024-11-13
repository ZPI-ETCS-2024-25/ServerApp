using EtcsServer.Configuration;
using Microsoft.Extensions.Options;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text.Json;

namespace EtcsServer.Security
{
    public class SecurityManager : ISecurityManager
    {
        private readonly IOptions<SecurityConfiguration> securityConfiguration;
        private byte[] key;
        private byte[] iv;

        public SecurityManager(IOptions<SecurityConfiguration> securityConfiguration)
        {
            this.securityConfiguration = securityConfiguration;
            this.key = Convert.FromBase64String(securityConfiguration.Value.Base64EncodedAesKey);
            this.iv = Convert.FromBase64String(securityConfiguration.Value.Base64EncodedInitialisationVector);
        }

        public string Encrypt(object data)
        {
            string json = JsonSerializer.Serialize(data);
            return Encrypt(json);
        }

        public string Encrypt(string plainText)
        {
            if (plainText == null || plainText.Length <= 0)
                return string.Empty;

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

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

            return Convert.ToBase64String(encrypted);
        }

        public T Decrypt<T>(string ciphertext)
        {
            string decrypted = Decrypt(ciphertext);
            return JsonSerializer.Deserialize<T>(decrypted)!;
        }

        public string Decrypt(string encodedCiphertext)
        {
            byte[] cipherText = Convert.FromBase64String(encodedCiphertext);
            if (cipherText == null || cipherText.Length <= 0)
                return string.Empty;


            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
