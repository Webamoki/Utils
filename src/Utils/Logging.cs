namespace Webamoki.Utils;

public enum LoggingLevel
{
    Info,
    Warn,
    Error
}

public static class Logging
{
    private static int _backingFieldAccess = 0;
    private static bool _isLoggingEnabled;

    public static bool IsLoggingEnabled
    {
        get => _isLoggingEnabled;
        set
        {
            if (_backingFieldAccess >= 1)
                throw new InvalidOperationException("Cannot change IsLoggingEnabled after it has been set");
            _isLoggingEnabled = value;
            _backingFieldAccess++;
        }
    }

    public static void WriteLog(string message,
        LoggingLevel level = LoggingLevel.Info,
        ConsoleColor foregroundColor = ConsoleColor.White)
    {
        switch (level)
        {
            case LoggingLevel.Info:
                Console.ForegroundColor = ConsoleColor.Blue;
                break;
            case LoggingLevel.Warn:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;
            case LoggingLevel.Error:
                Console.BackgroundColor = ConsoleColor.DarkRed;
                break;
            default:
                throw new ArgumentException("Unknown LoggingLevel value provided");
        }

        if (foregroundColor != ConsoleColor.White) Console.ForegroundColor = foregroundColor;

        Console.WriteLine($"\u25ba  {message}");
        Console.ResetColor();
    }

    // TODO: Check if this is being used anywhere for Production code
    public static void WriteDebugLog(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"#DEBUG#\u25ba  {message}");
        Console.ResetColor();
    }
}
