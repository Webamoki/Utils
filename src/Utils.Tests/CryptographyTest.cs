using NUnit.Framework;
using Webamoki.Utils;
using Webamoki.Utils.Testing;

namespace Utils.Tests;

public class CryptographyTest
{
    [TestCase(10, false)]
    [TestCase(20, true)]
    public void CreateToken_ShouldGenerateExpectedLengthToken(int length, bool isUppercase)
    {
        // Act
        var token = Cryptography.CreateToken(length, isUppercase);

        // Assert
        Ensure.Equal(length, token.Length);
        Ensure.Matches(isUppercase ? "^[A-Z0-9]+$" : "^[a-zA-Z0-9]+$", token);
    }

    [Test]
    public void Hash_ShouldReturnHashedString()
    {
        // Arrange
        const string value = "password123";

        // Act
        var hash = Cryptography.Hash(value);

        // Assert
        Ensure.True(Cryptography.Verify(value, hash));
    }

    [Test]
    public void Encode_ShouldReturnHexEncodedString()
    {
        // Arrange
        const string value = "test";
        const string expected = "s74657374"; // Hexadecimal encoding of "test"

        // Act
        var result = Cryptography.Encode(value);

        // Assert
        Ensure.Equal(expected, result);
    }

    [Test]
    public void TryDecode_AfterEncode_ShouldReturnOriginalString()
    {
        // Arrange
        var hexValue = Cryptography.Encode("test"); // Hexadecimal encoding of "test"
        const string expected = "test";

        // Act
        Cryptography.TryDecode(hexValue, out var decodedValue);

        // Assert
        Ensure.Equal(expected, decodedValue);
    }

    [Test]
    public void TryDecode_FakeString_ReturnFalse()
    {
        Ensure.False(Cryptography.TryDecode("fake", out _));
        Ensure.False(Cryptography.TryDecode("sxx", out _));
        Ensure.False(Cryptography.TryDecode("sade", out _));
    }


    [Test]
    public void TrySecureDecode_AfterSecureEncode_ShouldReturnOriginalString()
    {
        // Arrange
        const string transcoding = "ACCOUNT_DYNAMIC_MODEL";
        var value = Cryptography.SecureEncode("secret", transcoding);
        const string expected = "secret";

        // Act
        var success = Cryptography.TrySecureDecode(value, transcoding, out var decodedValue);

        // Assert
        Ensure.True(success);
        Ensure.Equal(expected, decodedValue);
    }

    [Test]
    public void TrySecureDecode_FakeString_ReturnFalse()
    {
        // Act
        var success = Cryptography.TrySecureDecode("fake string", "test", out _);

        // Assert
        Ensure.False(success);
    }


    [Test]
    public void IntToBaseX_ShouldReturnExpectedBaseXString()
    {
        // Arrange
        const int number = 39;
        const string baseXChars = "abcdefghijklmnopqrstuvwxyz0123456789-_";
        const string expected = "aa";

        // Act
        var result = Cryptography.IntToBaseX(number, baseXChars);

        // Assert
        Ensure.Equal(expected, result);
    }

    [Test]
    public void IntToBaseX_NegativeValue_ThrowException()
    {
        Ensure.Throws(() => Cryptography.IntToBaseX(-1, "sd"));
    }
}