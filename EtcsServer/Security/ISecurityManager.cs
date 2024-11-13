using EtcsServer.Configuration;
using Microsoft.Extensions.Options;
using System.Buffers.Text;
using System.Security.Cryptography;
using System.Text.Json;

namespace EtcsServer.Security
{
    public interface ISecurityManager
    {
        string Encrypt(object data);

        string Encrypt(string plainText);

        T Decrypt<T>(string ciphertext);

        string Decrypt(string encodedCiphertext);
    }
}
