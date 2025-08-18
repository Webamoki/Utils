using System.Text;
using NUnit.Framework;
using Webamoki.Utils;

namespace Utils.Tests;
public class EmbeddedResourceHandlerTest
{
    [Test]
    public void Read_ReturnsExpectedFileContent()
    {
        //Act
        using var fileContents = EmbeddedResourceHandler.Read(GetType().Assembly, "test.txt");

        //Assert
        //Remove UTF-8 BOM from start of text file
        using var streamReader = new StreamReader(fileContents!, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        var content = streamReader.ReadToEnd();
        Assert.That(content, Is.EqualTo("test"));
    }

    [Test]
    public void Exists_ReturnsTrue()
    {
        //Assert
        Assert.That(EmbeddedResourceHandler.Exists(GetType().Assembly, "test.txt"));
    }
}
