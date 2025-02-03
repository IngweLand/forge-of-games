using System.Security.Cryptography;
using System.Text;

namespace Ingweland.Fog.Shared.Utils;

public static class CryptoUtils
{
    private const int KEY_SIZE = 32; // 256 bits
    private const int NONCE_SIZE = 12;
    private const int TAG_SIZE = 16;

    public static string DecryptStringFromBytes(byte[] cipherText, byte[] key)
    {
        if (cipherText is not {Length: > NONCE_SIZE + TAG_SIZE})
        {
            throw new ArgumentException("Invalid cipherText", nameof(cipherText));
        }

        if (key is not {Length: KEY_SIZE})
        {
            throw new ArgumentException("Invalid key", nameof(key));
        }

        var nonce = new byte[NONCE_SIZE];
        var tag = new byte[TAG_SIZE];
        var encryptedData = new byte[cipherText.Length - NONCE_SIZE - TAG_SIZE];

        Buffer.BlockCopy(cipherText, 0, nonce, 0, NONCE_SIZE);
        Buffer.BlockCopy(cipherText, cipherText.Length - TAG_SIZE, tag, 0, TAG_SIZE);
        Buffer.BlockCopy(cipherText, NONCE_SIZE, encryptedData, 0, encryptedData.Length);

        using var aes = new AesGcm(key, TAG_SIZE);
        var plaintext = new byte[encryptedData.Length];

        aes.Decrypt(nonce, encryptedData, tag, plaintext);

        return Encoding.UTF8.GetString(plaintext);
    }

    public static byte[] EncryptStringToBytes(string plainText, byte[] key)
    {
        ArgumentException.ThrowIfNullOrEmpty(plainText, nameof(plainText));
        if (key is not {Length: KEY_SIZE})
        {
            throw new ArgumentException("Invalid key", nameof(key));
        }

        var nonce = new byte[NONCE_SIZE];
        RandomNumberGenerator.Fill(nonce);

        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        var cipherText = new byte[plainTextBytes.Length];
        var tag = new byte[TAG_SIZE];

        using (var aes = new AesGcm(key, TAG_SIZE))
        {
            aes.Encrypt(nonce, plainTextBytes, cipherText, tag);
        }

        var result = new byte[NONCE_SIZE + cipherText.Length + TAG_SIZE];
        Buffer.BlockCopy(nonce, 0, result, 0, NONCE_SIZE);
        Buffer.BlockCopy(cipherText, 0, result, NONCE_SIZE, cipherText.Length);
        Buffer.BlockCopy(tag, 0, result, NONCE_SIZE + cipherText.Length, TAG_SIZE);

        return result;
    }
}

public static class CryptoUtilExtensions
{
    private static byte[] DeriveKey(string token)
    {
        var salt = new byte[16];
        RandomNumberGenerator.Fill(salt);

        return Rfc2898DeriveBytes.Pbkdf2(token, salt, 10000, HashAlgorithmName.SHA256, 32);
    }

    public static string ToDecryptedString(this string payload, string token)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload, nameof(payload));
        ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));

        try
        {
            var key = DeriveKey(token);
            var cipherText = Convert.FromBase64String(payload);

            return CryptoUtils.DecryptStringFromBytes(cipherText, key);
        }
        catch (FormatException ex)
        {
            throw new ArgumentException("Invalid Base64 string.", nameof(payload), ex);
        }
        catch (CryptographicException ex)
        {
            throw new CryptographicException(
                "Decryption failed. The payload may be corrupted or the token may be incorrect.", ex);
        }
    }

    public static string ToEncryptedBase64String(this string payload, string token)
    {
        ArgumentException.ThrowIfNullOrEmpty(payload, nameof(payload));
        ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));

        try
        {
            var key = DeriveKey(token);
            var encryptedPayload = CryptoUtils.EncryptStringToBytes(payload, key);

            return Convert.ToBase64String(encryptedPayload);
        }
        catch (CryptographicException ex)
        {
            throw new CryptographicException("Encryption failed.", ex);
        }
    }
}
