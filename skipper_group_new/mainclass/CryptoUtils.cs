using System.Security.Cryptography;

public static class CryptoUtils
{
    public static string HashPassword(string password, int iterations = 100_000)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(password, 16, iterations, HashAlgorithmName.SHA256);
        byte[] salt = deriveBytes.Salt;
        byte[] key = deriveBytes.GetBytes(32); 

        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(key)}";
    }

    public static bool VerifyPassword(string password, string hashedPassword, int iterations = 100_000)
    {
        var parts = hashedPassword.Split(':');
        if (parts.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] key = Convert.FromBase64String(parts[1]);

        using var deriveBytes = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
        byte[] testKey = deriveBytes.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(key, testKey);
    }

    public static string Encrypt(string plainText, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var ms = new MemoryStream();
        using var encryptor = aes.CreateEncryptor();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public static string Decrypt(string cipherText, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
        using var decryptor = aes.CreateDecryptor();
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        return sr.ReadToEnd();
    }

    public static byte[] GenerateRandomKey(int size = 32)
    {
        byte[] key = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(key);
        return key;
    }

    public static byte[] GenerateRandomIV(int size = 16)
    {
        byte[] iv = new byte[size];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(iv);
        return iv;
    }
}
