namespace Webamoki.Utils;

public static class FileHandler
{
    private static readonly string BaseDirectory = AppContext.BaseDirectory;

    public static void Write(string path, string content)
    {
        var fullPath = BaseDirectory + $"/{path}";
        var directory = Path.GetDirectoryName(fullPath);

        if (directory != null) _ = Directory.CreateDirectory(directory);
        File.WriteAllText(fullPath, content);
    }

    public static string ReadText(string path) => File.ReadAllText(BaseDirectory + $"/{path}");

    public static byte[] ReadBytes(string path) => File.ReadAllBytes(BaseDirectory + $"/{path}");

    public static void Delete(string path, bool recursive = false)
    {
        var fullPath = BaseDirectory + $"/{path}";

        if (Directory.Exists(fullPath))
            Directory.Delete(fullPath, recursive);
        else if (File.Exists(fullPath)) File.Delete(fullPath);
    }

    public static bool Exists(string path) => File.Exists(BaseDirectory + $"/{path}");

    public static List<string> ListFiles(string directoryPath)
    {
        var fullPath = Path.Combine(BaseDirectory, directoryPath);

        if (Directory.Exists(fullPath))
            return [.. Directory.GetFiles(fullPath)
                .Select(file =>
                    Path.Combine(directoryPath,
                        Path.GetFileName(file)))];

        return [];
    }
}
