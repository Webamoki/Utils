using NUnit.Framework;
using Webamoki.Utils;
using Webamoki.Utils.Testing;

namespace Utils.Tests;

[TestFixture]
public class LoggingTest
{
    [SetUp]
    public void Setup()
    {
        // Reset logging state before each test
        Logging.Disable();
        Logging.ClearBuffer();
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up after each test
        Logging.Disable();
        Logging.ClearBuffer();
    }

    #region Enable/Disable Tests

    [Test]
    public void Enable_SetsLoggingToEnabled()
    {
        Logging.Enable();
        // We can't directly test _isEnabled, but we can test behavior
        // This test ensures Enable() doesn't throw
    }

    [Test]
    public void Disable_SetsLoggingToDisabled()
    {
        Logging.Enable();
        Logging.Disable();
        // This test ensures Disable() doesn't throw
    }

    [Test]
    public void Enable_CalledMultipleTimes_DoesNotThrow()
    {
        Logging.Enable();
        Logging.Enable();
        Logging.Enable();
        // Should not throw
    }

    [Test]
    public void Disable_CalledMultipleTimes_DoesNotThrow()
    {
        Logging.Enable();
        Logging.Disable();
        Logging.Disable();
        Logging.Disable();
        // Should not throw
    }

    #endregion

    #region WriteLog Tests

    [Test]
    public void WriteLog_WithoutLabel_DoesNotThrow()
    {
        Logging.Enable();
        Logging.WriteLog("Test message");
        Logging.WriteLog("Test message", LoggingLevel.Debug);
        Logging.WriteLog("Test message", LoggingLevel.Info, ConsoleColor.Green);
    }

    [Test]
    public void WriteLog_WhenDisabled_DoesNotThrow()
    {
        Logging.Disable();
        Logging.WriteLog("Test message");
        Logging.WriteLog("Test message", LoggingLevel.Error, ConsoleColor.Red);
    }

    [Test]
    public void WriteLog_WithLabel_BuffersLog()
    {
        const string label = "test-label";
        const string message = "Test message";

        Logging.WriteLog(message, LoggingLevel.Info, null, label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Single(heldLogs);
        Ensure.Equal(message, heldLogs[0].Message);
        Ensure.Equal(LoggingLevel.Info, heldLogs[0].Level);
    }

    [Test]
    public void WriteLog_WithLabelAndCustomColor_BuffersWithColor()
    {
        const string label = "color-test";
        const string message = "Colored message";

        Logging.WriteLog(message, LoggingLevel.Warn, ConsoleColor.Cyan, label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Single(heldLogs);
        Ensure.Equal(ConsoleColor.Cyan, heldLogs[0].ForegroundColor);
        Ensure.Equal(LoggingLevel.Warn, heldLogs[0].Level);
    }

    [Test]
    public void WriteLog_WithEmptyLabel_DoesNotBuffer()
    {
        Logging.WriteLog("Test message", LoggingLevel.Info, null, "");
        Logging.WriteLog("Test message");

        var labels = Logging.GetBufferLabels();
        Ensure.Empty(labels.ToList());
    }

    #endregion

    #region Specific Write Method Tests

    [Test]
    public void WriteDebug_WithoutLabel_DoesNotThrow()
    {
        Logging.Enable();
        Logging.WriteDebug("Debug message");
    }

    [Test]
    public void WriteDebug_WithLabel_BuffersWithDebugLevel()
    {
        const string label = "debug-test";
        const string message = "Debug message";

        Logging.WriteDebug(message, label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Single(heldLogs);
        Ensure.Equal(message, heldLogs[0].Message);
        Ensure.Equal(LoggingLevel.Debug, heldLogs[0].Level);
        Ensure.Equal(ConsoleColor.DarkMagenta, heldLogs[0].ForegroundColor);
    }

    [Test]
    public void WriteInfo_WithLabel_BuffersWithInfoLevel()
    {
        const string label = "info-test";
        const string message = "Info message";

        Logging.WriteInfo(message, label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Single(heldLogs);
        Ensure.Equal(message, heldLogs[0].Message);
        Ensure.Equal(LoggingLevel.Info, heldLogs[0].Level);
        Ensure.Equal(ConsoleColor.Blue, heldLogs[0].ForegroundColor);
    }

    [Test]
    public void WriteWarn_WithLabel_BuffersWithWarnLevel()
    {
        const string label = "warn-test";
        const string message = "Warning message";

        Logging.WriteWarn(message, label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Single(heldLogs);
        Ensure.Equal(message, heldLogs[0].Message);
        Ensure.Equal(LoggingLevel.Warn, heldLogs[0].Level);
        Ensure.Equal(ConsoleColor.Yellow, heldLogs[0].ForegroundColor);
    }

    [Test]
    public void WriteError_WithLabel_BuffersWithErrorLevel()
    {
        const string label = "error-test";
        const string message = "Error message";

        Logging.WriteError(message, label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Single(heldLogs);
        Ensure.Equal(message, heldLogs[0].Message);
        Ensure.Equal(LoggingLevel.Error, heldLogs[0].Level);
        Ensure.Equal(ConsoleColor.Red, heldLogs[0].ForegroundColor);
    }

    #endregion

    #region Hold Method Tests

    [Test]
    public void Hold_InitializesBuffer()
    {
        const string label = "test-label";

        Logging.Hold(label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Empty(heldLogs);
    }

    [Test]
    public void Hold_CalledMultipleTimes_DoesNotThrow()
    {
        const string label = "test-label";

        Logging.Hold(label);
        Logging.Hold(label);
        Logging.Hold(label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Empty(heldLogs);
    }

    [Test]
    public void Hold_DifferentLabels_CreatesMultipleBuffers()
    {
        Logging.Hold("label1");
        Logging.Hold("label2");
        Logging.Hold("label3");

        var labels = Logging.GetBufferLabels().ToList();
        Ensure.Count(labels, 3);
        Ensure.True(labels.Contains("label1"));
        Ensure.True(labels.Contains("label2"));
        Ensure.True(labels.Contains("label3"));
    }

    #endregion

    #region Buffer Management Tests

    [Test]
    public void GetHeldLogs_NonExistentLabel_ReturnsEmptyList()
    {
        var heldLogs = Logging.GetHeldLogs("non-existent");
        Ensure.Empty(heldLogs);
    }

    [Test]
    public void GetHeldLogs_ExistingLabel_ReturnsLogsCopy()
    {
        const string label = "test-label";
        const string message1 = "Message 1";
        const string message2 = "Message 2";

        Logging.WriteLog(message1, label: label);
        Logging.WriteLog(message2, label: label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Count(heldLogs, 2);
        Ensure.Equal(message1, heldLogs[0].Message);
        Ensure.Equal(message2, heldLogs[1].Message);

        // Verify it's a copy by modifying the returned list
        heldLogs.Clear();
        var heldLogsAgain = Logging.GetHeldLogs(label);
        Ensure.Count(heldLogsAgain, 2); // Original should be unchanged
    }

    [Test]
    public void WriteLog_MultipleSameLabel_AccumulatesLogs()
    {
        const string label = "accumulate-test";

        Logging.WriteLog("Message 1", label: label);
        Logging.WriteLog("Message 2", LoggingLevel.Warn, label: label);
        Logging.WriteLog("Message 3", LoggingLevel.Error, ConsoleColor.Red, label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Count(heldLogs, 3);
        Ensure.Equal("Message 1", heldLogs[0].Message);
        Ensure.Equal("Message 2", heldLogs[1].Message);
        Ensure.Equal("Message 3", heldLogs[2].Message);
        Ensure.Equal(LoggingLevel.Info, heldLogs[0].Level);
        Ensure.Equal(LoggingLevel.Warn, heldLogs[1].Level);
        Ensure.Equal(LoggingLevel.Error, heldLogs[2].Level);
    }

    [Test]
    public void WriteLog_DifferentLabels_StoresSeparately()
    {
        Logging.WriteLog("Message for label 1", label: "label1");
        Logging.WriteLog("Message for label 2", label: "label2");

        var logs1 = Logging.GetHeldLogs("label1");
        var logs2 = Logging.GetHeldLogs("label2");

        Ensure.Single(logs1);
        Ensure.Single(logs2);
        Ensure.Equal("Message for label 1", logs1[0].Message);
        Ensure.Equal("Message for label 2", logs2[0].Message);
    }

    [Test]
    public void WriteLog_WithoutLabel_DoesNotBuffer()
    {
        Logging.WriteLog("Message without label");

        var allLabels = Logging.GetBufferLabels();
        Ensure.Empty(allLabels.ToList());
    }

    [Test]
    public void ClearHeldLogs_RemovesLogsForSpecificLabel()
    {
        const string label1 = "label1";
        const string label2 = "label2";

        Logging.WriteLog("Message 1", label: label1);
        Logging.WriteLog("Message 2", label: label2);

        Logging.ClearHeldLogs(label1);

        var logs1 = Logging.GetHeldLogs(label1);
        var logs2 = Logging.GetHeldLogs(label2);

        Ensure.Empty(logs1);
        Ensure.Single(logs2);
    }

    [Test]
    public void ClearHeldLogs_NonExistentLabel_DoesNotThrow()
    {
        Logging.ClearHeldLogs("non-existent");
        // Should not throw
    }

    [Test]
    public void ClearBuffer_RemovesAllBufferedLogs()
    {
        Logging.WriteLog("Message 1", label: "label1");
        Logging.WriteLog("Message 2", label: "label2");
        Logging.WriteLog("Message 3", label: "label3");

        Logging.ClearBuffer();

        var logs1 = Logging.GetHeldLogs("label1");
        var logs2 = Logging.GetHeldLogs("label2");
        var logs3 = Logging.GetHeldLogs("label3");

        Ensure.Empty(logs1);
        Ensure.Empty(logs2);
        Ensure.Empty(logs3);

        var labels = Logging.GetBufferLabels();
        Ensure.Empty(labels.ToList());
    }

    [Test]
    public void GetBufferLabels_ReturnsAllLabels()
    {
        Logging.WriteLog("Message 1", label: "label1");
        Logging.WriteLog("Message 2", label: "label2");
        Logging.Hold("label3"); // Empty buffer

        var labels = Logging.GetBufferLabels().ToList();

        Ensure.Count(labels, 3);
        Ensure.True(labels.Contains("label1"));
        Ensure.True(labels.Contains("label2"));
        Ensure.True(labels.Contains("label3"));
    }

    [Test]
    public void GetBufferLabels_NoBufferedLogs_ReturnsEmptyCollection()
    {
        var labels = Logging.GetBufferLabels();
        Ensure.Empty(labels.ToList());
    }

    #endregion

    #region LogEntry Tests

    [Test]
    public void LogEntry_HasCorrectTimestamp()
    {
        const string label = "timestamp-test";
        var beforeTime = DateTime.Now;

        Logging.WriteLog("Test message", label: label);

        var afterTime = DateTime.Now;
        var heldLogs = Logging.GetHeldLogs(label);

        Ensure.Single(heldLogs);
        var logEntry = heldLogs[0];
        Ensure.True(logEntry.Timestamp >= beforeTime);
        Ensure.True(logEntry.Timestamp <= afterTime);
    }

    [Test]
    public void LogEntry_Record_PropertiesAreCorrect()
    {
        var timestamp = DateTime.Now;
        var logEntry = new LogEntry(timestamp, LoggingLevel.Warn, "Test message", ConsoleColor.Cyan);

        Ensure.Equal(timestamp, logEntry.Timestamp);
        Ensure.Equal(LoggingLevel.Warn, logEntry.Level);
        Ensure.Equal("Test message", logEntry.Message);
        Ensure.Equal(ConsoleColor.Cyan, logEntry.ForegroundColor);
    }

    [Test]
    public void LogEntry_WithNullColor_HasNullForegroundColor()
    {
        var logEntry = new LogEntry(DateTime.Now, LoggingLevel.Info, "Test message");
        Ensure.Null(logEntry.ForegroundColor);
    }

    [Test]
    public void LogEntry_DefaultColor_IsNull()
    {
        var logEntry = new LogEntry(DateTime.Now, LoggingLevel.Info, "Test message");
        Ensure.Null(logEntry.ForegroundColor);
    }

    #endregion

    #region Edge Cases and Integration Tests

    [Test]
    public void WriteLog_BuffersRegardlessOfLoggingState()
    {
        const string label = "state-test";

        // Test with logging disabled
        Logging.Disable();
        Logging.WriteLog("Message when disabled", label: label);

        // Test with logging enabled
        Logging.Enable();
        Logging.WriteLog("Message when enabled", label: label);

        var heldLogs = Logging.GetHeldLogs(label);
        Ensure.Count(heldLogs, 2);
        Ensure.Equal("Message when disabled", heldLogs[0].Message);
        Ensure.Equal("Message when enabled", heldLogs[1].Message);
    }

    [Test]
    public void WriteLog_EmptyMessage_DoesNotThrow()
    {
        Logging.WriteLog("");
        Logging.WriteLog("", label: "empty-test");

        var heldLogs = Logging.GetHeldLogs("empty-test");
        Ensure.Single(heldLogs);
        Ensure.Equal("", heldLogs[0].Message);
    }

    [Test]
    public void Hold_EmptyLabel_DoesNotThrow()
    {
        Logging.Hold("");
        var labels = Logging.GetBufferLabels();
        Ensure.True(labels.Contains(""));
    }

    [Test]
    public void WriteLog_EmptyLabel_DoesNotBuffer()
    {
        Logging.WriteLog("Test message", label: "");
        Logging.WriteLog("Test message", label: null);

        var labels = Logging.GetBufferLabels();
        Ensure.Empty(labels.ToList());
    }

    [Test]
    public void AllLogLevels_HaveCorrectEnumValues()
    {
        Ensure.Equal(0, (int)LoggingLevel.Debug);
        Ensure.Equal(1, (int)LoggingLevel.Info);
        Ensure.Equal(2, (int)LoggingLevel.Warn);
        Ensure.Equal(3, (int)LoggingLevel.Error);
    }

    [Test]
    public void ConcurrentAccess_MultipleThreads_DoesNotThrow()
    {
        const int threadCount = 10;
        const int messagesPerThread = 100;
        var tasks = new Task[threadCount];

        for (int i = 0; i < threadCount; i++)
        {
            int threadId = i;
            tasks[i] = Task.Run(() =>
            {
                for (int j = 0; j < messagesPerThread; j++)
                {
                    Logging.WriteLog($"Thread {threadId} Message {j}", label: $"thread-{threadId}");
                    Logging.Hold($"hold-{threadId}-{j}");
                    Logging.WriteDebug($"Debug {j}", $"debug-{threadId}");
                }
            });
        }

        Task.WaitAll(tasks);

        // Verify some logs were created
        var labels = Logging.GetBufferLabels().ToList();
        Ensure.True(labels.Count > 0);
    }

    #endregion
}
