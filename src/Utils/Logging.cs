namespace Webamoki.Utils;

public enum LoggingLevel
{
    Debug,
    Info,
    Warn,
    Error
}

public record LogEntry(DateTime Timestamp, LoggingLevel Level, string Message, ConsoleColor? ForegroundColor = null);

public static class Logging
{
    private static bool _isEnabled;
    private static readonly Dictionary<string, List<LogEntry>> LogBuffer = new();
    private static readonly Lock BufferLock = new();
    

    /// <summary>
    /// Enable logging. Safe to call multiple times.
    /// </summary>
    public static void Enable()
    {
        _isEnabled = true;
    }

    /// <summary>
    /// Disable logging. Safe to call multiple times.
    /// </summary>
    public static void Disable()
    {
        _isEnabled = false;
    }

    /// <summary>
    /// Write a log message to console if logging is enabled, optionally buffer it with a label
    /// </summary>
    /// <param name="message">The message to log</param>
    /// <param name="level">The logging level</param>
    /// <param name="foregroundColor">Optional custom foreground color</param>
    /// <param name="label">Optional label to buffer the log for later retrieval</param>
    public static void WriteLog(string message,
        LoggingLevel level = LoggingLevel.Info,
        ConsoleColor? foregroundColor = null,
        string? label = null)
    {
        var logEntry = new LogEntry(DateTime.Now, level, message, foregroundColor);

        // Buffer the log if a label is provided
        if (!string.IsNullOrEmpty(label))
        {
            lock (BufferLock)
            {
                if (!LogBuffer.ContainsKey(label))
                {
                    LogBuffer[label] = [];
                }
                LogBuffer[label].Add(logEntry);
            }
        }
        // Write to console if logging is enabled
        if (_isEnabled)
        {
            WriteLogEntry(logEntry);
        }
    }

    /// <summary>
    /// Write a debug log message
    /// </summary>
    /// <param name="message">The debug message to log</param>
    /// <param name="label">Optional label to buffer the log for later retrieval</param>
    public static void WriteDebug(string message, string? label = null)
    {
        WriteLog(message, LoggingLevel.Debug, ConsoleColor.DarkMagenta, label);
    }

    /// <summary>
    /// Write an info log message
    /// </summary>
    /// <param name="message">The info message to log</param>
    /// <param name="label">Optional label to buffer the log for later retrieval</param>
    public static void WriteInfo(string message, string? label = null)
    {
        WriteLog(message, LoggingLevel.Info, ConsoleColor.Blue, label);
    }

    /// <summary>
    /// Write a warning log message
    /// </summary>
    /// <param name="message">The warning message to log</param>
    /// <param name="label">Optional label to buffer the log for later retrieval</param>
    public static void WriteWarn(string message, string? label = null)
    {
        WriteLog(message, LoggingLevel.Warn, ConsoleColor.Yellow, label);
    }

    /// <summary>
    /// Write an error log message
    /// </summary>
    /// <param name="message">The error message to log</param>
    /// <param name="label">Optional label to buffer the log for later retrieval</param>
    public static void WriteError(string message, string? label = null)
    {
        WriteLog(message, LoggingLevel.Error, ConsoleColor.Red, label);
    }

    /// <summary>
    /// Initialize a buffer with a specific label for holding logs
    /// </summary>
    /// <param name="label">The label to associate with buffered logs</param>
    public static void Hold(string label)
    {
        lock (BufferLock)
        {
            if (!LogBuffer.ContainsKey(label))
            {
                LogBuffer[label] = [];
            }
        }
    }

    /// <summary>
    /// Retrieve all buffered logs for a specific label
    /// </summary>
    /// <param name="label">The label to retrieve logs for</param>
    /// <returns>List of log entries for the label, or empty list if label doesn't exist</returns>
    public static List<LogEntry> GetHeldLogs(string label)
    {
        lock (BufferLock)
        {
            return LogBuffer.TryGetValue(label, out var logs)
                ? [..logs]
                : [];
        }
    }

    /// <summary>
    /// Clear all buffered logs for a specific label
    /// </summary>
    /// <param name="label">The label to clear logs for</param>
    public static void ClearHeldLogs(string label)
    {
        lock (BufferLock)
        {
            LogBuffer.Remove(label, out _);
        }
    }

    /// <summary>
    /// Get all available buffer labels
    /// </summary>
    /// <returns>Collection of all buffer label names</returns>
    public static IEnumerable<string> GetBufferLabels()
    {
        lock (BufferLock)
        {
            return LogBuffer.Keys.ToList();
        }
    }

    /// <summary>
    /// Clear all buffered logs
    /// </summary>
    public static void ClearBuffer()
    {
        lock (BufferLock)
        {
            LogBuffer.Clear();
        }
    }

    private static void WriteLogEntry(LogEntry logEntry)
    {
        // Set color based on level or custom color
        var color = logEntry.ForegroundColor ?? GetDefaultColorForLevel(logEntry.Level);
        Console.ForegroundColor = color;

        // Format timestamp
        var timestamp = logEntry.Timestamp.ToString("HH:mm:ss.fff");
        var levelText = logEntry.Level.ToString().ToUpper().PadRight(5);

        // Write the log entry
        Console.WriteLine($"[{timestamp}] {levelText} \u25ba {logEntry.Message}");
        Console.ResetColor();
    }

    private static ConsoleColor GetDefaultColorForLevel(LoggingLevel level)
    {
        return level switch
        {
            LoggingLevel.Debug => ConsoleColor.DarkMagenta,
            LoggingLevel.Info => ConsoleColor.Blue,
            LoggingLevel.Warn => ConsoleColor.Yellow,
            LoggingLevel.Error => ConsoleColor.Red,
            _ => ConsoleColor.White
        };
    }
}
