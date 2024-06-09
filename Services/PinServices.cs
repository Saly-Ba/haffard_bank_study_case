using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace HaffardBankApi.Services;


public interface IPinService
{
    string GeneratePin();
    string EncryptPin(string pin);
    string DecryptPin(string encryptedPin);
}

public class PinService : IPinService{

    private readonly IConfiguration _configuration;

    public PinService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GeneratePin()
    {
        var random = new Random();
        return random.Next(1000, 9999).ToString();
    }

    public string EncryptPin(string pin)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(_configuration["EncryptionKey"]);
            aes.IV = Encoding.UTF8.GetBytes(_configuration["EncryptionIV"]);

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(pin);
                    }
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public string DecryptPin(string encryptedPin)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(_configuration["EncryptionKey"]);
            aes.IV = Encoding.UTF8.GetBytes(_configuration["EncryptionIV"]);

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream(Convert.FromBase64String(encryptedPin)))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }

}