using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages debug logging to text files with date-based organization and multiple session support.
/// Automatically creates new log files for different dates and handles multiple sessions within the same day.
/// </summary>
public class DebugLogTextFile : MonoBehaviour
{
    [Tooltip("Base name for log files - will be combined with date and session number")]
    [SerializeField] private string baseLogFileName = "Rogue-Like-Game-Logs";


    [Tooltip("Name of the folder where log files will be stored")]
    [SerializeField] private string logFolderName = "Logs";


    [Tooltip("Maximum number of session log files allowed per day")]
    [SerializeField] private int maxLogFilesPerDay = 100;


    [Tooltip("Number of days to keep log files before automatic deletion")]
    [SerializeField] private int daysToKeepLogs = 7;


    // Path to the current log file
    private string logFilePath;


    // Path to the folder containing all log files
    private string logFolderPath;


    // Thread synchronization object for file writing
    private readonly object fileLock = new object();


    // Current date for file organization
    private string currentDate;


    /// <summary>
    /// Initializes the logging system on GameObject awakening.
    /// Sets up necessary paths and performs initial cleanup.
    /// </summary>
    private void Awake()
    {
        InitializeLogPaths();
        EnsureLogDirectoryExists();
        CleanupOldLogs();
        InitializeCurrentLogFile();
        LogSystemInfo();
    }

    /// <summary>
    /// Subscribes to Unity's log message events when the component is enabled.
    /// </summary>
    private void OnEnable()
    {
        Application.logMessageReceived += LogMessage;
    }

    /// <summary>
    /// Unsubscribes from Unity's log message events when the component is disabled.
    /// </summary>
    private void OnDisable()
    {
        Application.logMessageReceived -= LogMessage;
    }

    /// <summary>
    /// Initializes the paths for log files and sets the current date.
    /// Uses Application.persistentDataPath for cross-platform compatibility.
    /// </summary>
    private void InitializeLogPaths()
    {
        logFolderPath = Path.Combine(Application.persistentDataPath, logFolderName);
        currentDate = DateTime.Now.ToString("dd-MM-yyyy");
    }

    /// <summary>
    /// Ensures the log directory exists, creates it if it doesn't.
    /// </summary>
    private void EnsureLogDirectoryExists()
    {
        try
        {
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
                Debug.Log($"Created log directory at: {logFolderPath}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to create log directory: {e.Message}");
        }
    }

    /// <summary>
    /// Initializes the current log file. Either creates a new file or continues writing to an existing one.
    /// Handles multiple sessions within the same day using session numbers.
    /// </summary>
    private void InitializeCurrentLogFile()
    {
        try
        {
            string baseFileName = $"{baseLogFileName}_{currentDate}";
            // Get all existing log files for today
            string[] existingFiles = Directory.GetFiles(logFolderPath, $"{baseFileName}*.txt")
                                            .OrderBy(f => f)
                                            .ToArray();

            if (existingFiles.Length > 0)
            {
                string lastFile = existingFiles.Last();
                if (IsFileLocked(lastFile))
                {
                    // Create new session file if last one is locked
                    CreateNewSessionFile(baseFileName, existingFiles.Length);
                }
                else
                {
                    // Continue writing to existing file
                    logFilePath = lastFile;
                    WriteToFile("\n----------------------------------------");
                    WriteToFile($"Log Continued - New Session: {DateTime.Now}");
                    WriteToFile("----------------------------------------\n");
                }
            }
            else
            {
                // Create first session file for today
                CreateNewSessionFile(baseFileName, 0);
            }

            Debug.Log($"Initialized log file at: {logFilePath}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to initialize log file: {e.Message}");
        }
    }

    /// <summary>
    /// Creates a new session file with an incremented session number.
    /// </summary>
    /// <param name="baseFileName">Base name for the log file</param>
    /// <param name="existingFileCount">Number of existing session files for today</param>
    private void CreateNewSessionFile(string baseFileName, int existingFileCount)
    {
        if (existingFileCount >= maxLogFilesPerDay)
        {
            Debug.LogError($"Maximum number of log files ({maxLogFilesPerDay}) for today has been reached.");
            return;
        }

        string sessionNumber = (existingFileCount + 1).ToString("D2");
        logFilePath = Path.Combine(logFolderPath, $"{baseFileName}_Session{sessionNumber}.txt");
    }

    /// <summary>
    /// Checks if a file is locked (being used by another process).
    /// </summary>
    /// <param name="filePath">Path to the file to check</param>
    /// <returns>True if the file is locked, false otherwise</returns>
    private bool IsFileLocked(string filePath)
    {
        try
        {
            using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                stream.Close();
            }
            return false;
        }
        catch (IOException)
        {
            return true;
        }
    }

    /// <summary>
    /// Cleans up log files older than the specified retention period.
    /// </summary>
    private void CleanupOldLogs()
    {
        try
        {
            var currentDate = DateTime.Now;
            var files = Directory.GetFiles(logFolderPath, $"{baseLogFileName}*.txt");

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                var fileAge = currentDate - fileInfo.CreationTime;

                if (fileAge.TotalDays > daysToKeepLogs)
                {
                    try
                    {
                        File.Delete(file);
                        Debug.Log($"Deleted old log file: {file}");
                    }
                    catch (IOException)
                    {
                        Debug.LogWarning($"Could not delete log file: {file} - file may be in use");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to cleanup old logs: {e.Message}");
        }
    }

    /// <summary>
    /// Logs system information at the start of each session.
    /// Includes OS, device, and application details.
    /// </summary>
    private void LogSystemInfo()
    {
        string[] systemInfo = new[]
        {
            $"Game Started: {DateTime.Now}",
            $"Operating System: {SystemInfo.operatingSystem}",
            $"Processor: {SystemInfo.processorType}",
            $"Memory: {SystemInfo.systemMemorySize}MB",
            $"Graphics Device: {SystemInfo.graphicsDeviceName}",
            $"Graphics Memory: {SystemInfo.graphicsMemorySize}MB",
            $"Application Version: {Application.version}",
            "----------------------------------------"
        };

        WriteToFile(string.Join(Environment.NewLine, systemInfo));
    }

    /// <summary>
    /// Handles incoming log messages from Unity's logging system.
    /// Checks for date changes and formats the message before writing.
    /// </summary>
    /// <param name="logString">The message to log</param>
    /// <param name="stackTrace">Stack trace for errors/exceptions</param>
    /// <param name="type">Type of log message</param>
    private void LogMessage(string logString, string stackTrace, LogType type)
    {
        try
        {
            // Check for date change
            string newDate = DateTime.Now.ToString("dd-MM-yyyy");
            if (newDate != currentDate)
            {
                currentDate = newDate;
                InitializeCurrentLogFile();
            }

            string formattedMessage = FormatLogMessage(logString, stackTrace, type);
            WriteToFile(formattedMessage);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to log file: {e.Message}");
        }
    }

    /// <summary>
    /// Formats a log message with timestamp, type, and optional stack trace.
    /// </summary>
    /// <param name="logString">The message to format</param>
    /// <param name="stackTrace">Stack trace for errors/exceptions</param>
    /// <param name="type">Type of log message</param>
    /// <returns>Formatted log message</returns>
    private string FormatLogMessage(string logString, string stackTrace, LogType type)
    {
        string timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.fff");
        string message = $"[{timestamp}] [{type}] {logString}";

        if (type == LogType.Exception || type == LogType.Error)
        {
            message += $"{Environment.NewLine}Stack Trace: {stackTrace}";
        }

        return message;
    }

    /// <summary>
    /// Writes a message to the log file in a thread-safe manner.
    /// </summary>
    /// <param name="message">Message to write to the file</param>
    private void WriteToFile(string message)
    {
        lock (fileLock)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to write to log file: {e.Message}");
            }
        }
    }

    /// <summary>
    /// Logs application quit message when the application is shutting down.
    /// </summary>
    private void OnApplicationQuit()
    {
        WriteToFile($"Application Quit: {DateTime.Now}\n----------------------------------------");
    }
}