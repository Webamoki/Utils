using NUnit.Framework;
using Webamoki.Utils;
using Webamoki.Utils.Testing;

namespace Utils.Tests;

public class FileHandlerTest
{
    [Test]
    public void CreateFile_WithContent_CreatesFile()
    {
        FileHandler.Write("test.txt", "test");
        Ensure.True(FileHandler.Exists("test.txt"));
        Ensure.Equal("test", FileHandler.ReadText("test.txt"));

        var testBytes = FileHandler.ReadBytes("test.txt");
        Ensure.Equal("test"u8.ToArray(), testBytes);

        FileHandler.Delete("test.txt");
        Ensure.False(File.Exists("test.txt"));
    }
}