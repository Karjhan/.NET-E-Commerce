using System.Diagnostics;

namespace Backend_API.Helpers;

public static class CommandRunner
{
    private static Process cmdProcess;

    public static int WebHookCount = 0;

    public static void StartCmd(string folderPath, string command)
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd.exe",
            WorkingDirectory = folderPath,
            Arguments = $"/C {command}",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };
        cmdProcess = new Process { StartInfo = processStartInfo };
        cmdProcess.Start();
        // Start a separate thread to read the output/error streams to prevent deadlocks
        ThreadPool.QueueUserWorkItem(state =>
        {
            string output = cmdProcess.StandardOutput.ReadToEnd();
            string error = cmdProcess.StandardError.ReadToEnd();
            // Process or log the output and error if needed
        });
    }

    public static void StopCmd()
    {
        if (cmdProcess != null && !cmdProcess.HasExited)
        {
            cmdProcess.CloseMainWindow();
            cmdProcess.Close();
            cmdProcess.Dispose();
        }
    }
    
    public static string RunCommand(string command, string workingDirectory)
    {
        string output = string.Empty;
        try
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/C {command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,   
                UseShellExecute = false,
                WorkingDirectory = workingDirectory
            };
            using (Process process = new Process())
            {
                process.StartInfo = processStartInfo;
                process.OutputDataReceived += (sender, e) => output += e.Data;
                process.ErrorDataReceived += (sender, e) => output += e.Data;
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }
        catch (Exception ex)
        {
            output = $"Error: {ex.Message}";
        }
        return output;
    }
}