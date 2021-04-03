using System.Diagnostics;
using Serilog;

namespace CandorUpdater.Main
{
    public static class CandorStarter
    {
        public static void StartCandor()
        {
            Log.Information("We are attempting to start candor now!");
            var process = new Process
            {
                StartInfo =
                {
                    // TODO: find java path or download it
                    FileName = "java",
                    Arguments = "-jar CandorManager-snapshot.jar",
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    ErrorDialog = true
                }
            };
            process.OutputDataReceived += (sender, data) => 
            {
                Log.ForContext<Process>().Information(data.Data);
            };
            process.ErrorDataReceived += (sender, data) =>
            {
                Log.ForContext<Process>().Error(data.Data);
            };
            if (process.Start())
            {
                process.BeginOutputReadLine();
                process.BeginErrorRealLine();
                Log.Debug("We have started candor!");
            }
            else
            {
                Log.Error("Unable to start candor.");
            }
            
            process.WaitForExitAsync();
            process.CancelOutputRead();
            string output = process.StandardOutput.ReadToEnd();
            Log.Information(output);
        }
    }
}
