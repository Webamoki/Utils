using NUnit.Framework;
using Webamoki.Utils;
using Webamoki.Utils.Testing;

namespace Utils.Tests;

public class StringExtensionsTest
{
    [Test]
    public void TrimEnd()
    {
        const string str = "test";
        Ensure.Equal("t", str.TrimEnd("est"));
        Ensure.Throws(() => str.TrimEnd("a"));
    }
}