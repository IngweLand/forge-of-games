namespace Ingweland.Fog.Shared.Utils;

public static class SimpleEncryption 
{
    public static string Encrypt(string text, string key)
    {
        if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(key)) 
            return text;

        var result = new char[text.Length];
        int keyLength = key.Length;
        
        // Simple XOR with cycling key + position-based shift
        for (int i = 0; i < text.Length; i++)
        {
            char keyChar = key[i % keyLength];
            int shift = (i % 7) + 1; // Additional shift based on position
            result[i] = (char)(text[i] ^ keyChar ^ shift);
        }

        // Convert to base64 to make it string-safe
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(new string(result)));
    }

    public static string Decrypt(string encryptedText, string key)
    {
        if (string.IsNullOrEmpty(encryptedText) || string.IsNullOrEmpty(key))
            return encryptedText;

        try
        {
            // Convert from base64
            var bytes = Convert.FromBase64String(encryptedText);
            var text = System.Text.Encoding.UTF8.GetString(bytes);
            
            var result = new char[text.Length];
            int keyLength = key.Length;

            // Reverse the XOR operation
            for (int i = 0; i < text.Length; i++)
            {
                char keyChar = key[i % keyLength];
                int shift = (i % 7) + 1;
                result[i] = (char)(text[i] ^ keyChar ^ shift);
            }

            return new string(result);
        }
        catch
        {
            return encryptedText; // Return original if decryption fails
        }
    }
}

// Extension methods for convenience
public static class StringEncryptionExtensions
{
    public static string EncryptSimple(this string text, string key) 
        => SimpleEncryption.Encrypt(text, key);

    public static string DecryptSimple(this string text, string key) 
        => SimpleEncryption.Decrypt(text, key);
}
