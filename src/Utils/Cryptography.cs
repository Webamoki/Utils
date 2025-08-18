using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable InconsistentNaming

namespace Webamoki.Utils;

public static partial class Cryptography
{
    private const string _uppercasePool = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string _pool = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [GeneratedRegex(@"\A\b[0-9a-fA-F]+\b\Z")]
    private static partial Regex IsHexadecimalRegex();

    /// <param name="length">Length of token</param>
    /// <param name="isUppercase">if true, token will only contain uppercase letters and numbers</param>
    /// <returns>random assortment of alphanumeric characters</returns>
    public static string CreateToken(int length, bool isUppercase = false)
    {
        var selectedPool = isUppercase ? _uppercasePool : _pool;
        var token = new char[length];
        var max = selectedPool.Length;

        for (var i = 0; i < length; i++) token[i] = selectedPool[CryptoRandSecure(max)];

        return new string(token);
    }

    private static int CryptoRandSecure(int max)
    {
        var range = max - 0;
        var log = Math.Log(range, 2);
        var bytes = (int)(log / 8 + 1);
        var bits = (int)(log + 1);
        var filter = (1 << bits) - 1;
        int rnd;

        do
        {
            rnd = 0;
            var randomBytes = RandomNumberGenerator.GetBytes(bytes);
            for (var i = 0; i < randomBytes.Length; i++) rnd |= randomBytes[i] << (8 * i);

            rnd &= filter;
        } while (rnd >= range);

        return 0 + rnd;
    }

    /// <summary>
    ///     One way hashing function that outputs a different result every time
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string Hash(string value)
    {
        const int cost = 10;
        return Encode(BCrypt.Net.BCrypt.HashPassword(value, cost));
    }

    /// <summary>
    ///     2-way encryption function that outputs same result every time and is extremely fast.
    ///     Do not use if you need to pass sensitive data
    /// </summary>
    /// <param name="value"></param>
    /// <returns>Only alphanumeric characters</returns>
    public static string Encode(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        // In HTML, ids and classes cannot start with a number, they must start with a letter, s is arbitrarily chosen
        var hexString = new StringBuilder("s");
        foreach (var b in bytes) hexString.Append(b.ToString("x2"));
        return hexString.ToString();
    }

    private static string Encode(byte[] value)
    {
        var hexString = new StringBuilder();
        foreach (var b in value) hexString.Append(b.ToString("x2"));
        return Encode(hexString.ToString());
    }

    /// <summary>
    ///     2-way encryption function that outputs a different result every time
    /// </summary>
    /// <param name="value"></param>
    /// <param name="transcodingType"></param>
    /// <returns></returns>
    public static string SecureEncode(string value, string transcodingType)
    {
        // TODO: Add Token to Config
        const string token = "asFo2iAS(m2ASN";
        var keyString = token + transcodingType;

        //Create iv Cipher for AES-256-CBC
        using var aes = Aes.Create();
        //Ensure the key is 32 bytes for AES-256
        aes.Key = Encoding.UTF8.GetBytes(keyString.PadRight(32)[..32]);
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.GenerateIV();
        var iv = aes.IV;

        //encrypt value
        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        var inputBytes = Encoding.UTF8.GetBytes(value);
        var encryptedBytes = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);

        //combine iv and encryptedBytes
        var result = new byte[iv.Length + encryptedBytes.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(encryptedBytes, 0, result, iv.Length, encryptedBytes.Length);
        return Encode(result);
    }

    /// <summary>
    ///     Compares a non hashed value to a hashed value to check if they match
    /// </summary>
    /// <param name="value"></param>
    /// <param name="hash"></param>
    /// <returns></returns>
    public static bool Verify(string value, string hash)
    {
        return TryDecode(hash, out var decodedValue) && BCrypt.Net.BCrypt.Verify(value, decodedValue);
    }

    private static byte[] ConvertHexadecimalStringToByteArray(string hexadecimal)
    {
        var length = hexadecimal.Length;
        var bytes = new byte[length / 2];

        for (var i = 0; i < length; i += 2)
            bytes[i / 2] = Convert.ToByte(hexadecimal.Substring(i, 2), 16);

        return bytes;
    }

    public static bool TryDecode(string value, out string decodedValue)
    {
        decodedValue = value;
        if (!value.StartsWith('s')) return false;
        value = value[1..];
        if (!IsHexadecimalRegex().IsMatch(value)) return false;
        if (value.Length % 2 != 0) return false;

        decodedValue = Encoding.UTF8.GetString(ConvertHexadecimalStringToByteArray(value));
        return true;
    }

    public static bool TrySecureDecode(string value, string transcodingType, out string decodedValue)
    {
        byte[] decryptedData;
        if (TryDecode(value, out decodedValue))
            decryptedData = ConvertHexadecimalStringToByteArray(decodedValue);
        else return false;

        // TODO: Add Token to Config
        const string token = "asFo2iAS(m2ASN";
        var keyString = token + transcodingType;

        //Create iv Cipher for AES-256-CBC
        using var aes = Aes.Create();
        //Ensure the key is 32 bytes for AES-256
        aes.Key = Encoding.UTF8.GetBytes(keyString.PadRight(32)[..32]);
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        //Extract the IV and ciphertext
        var ivLength = aes.BlockSize / 8; //AES block size is 16 bytes
        var iv = new byte[ivLength];
        var ciphertext = new byte[decryptedData.Length - ivLength];

        Buffer.BlockCopy(decryptedData, 0, iv, 0, ivLength);
        Buffer.BlockCopy(decryptedData, ivLength, ciphertext, 0, ciphertext.Length);

        using var decryptor = aes.CreateDecryptor(aes.Key, iv);
        var decryptedBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
        decodedValue = Encoding.UTF8.GetString(decryptedBytes);
        return true;
    }

    public static string IntToBaseX(int number, string baseXCharacters)
    {
        if (number <= 0) throw new ArgumentException("Number must be greater than 0");

        var baseX = baseXCharacters.Length;
        var result = "";

        while (number > 0)
        {
            var remainder = (number - 1) % baseX;
            result = baseXCharacters[remainder] + result;
            number = (number - 1) / baseX;
        }

        return result;
    }
}