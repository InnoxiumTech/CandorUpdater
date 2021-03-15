using System;
using System.Diagnostics;
using Serilog;

namespace CandorUpdater.Main
{
    public static class CandorStarter
    {
        public static void StartCandor()
        {
            // TODO: this is WIP and likely will not work, we need to change the build procedure for candor first
            Log.Information("We are attempting to start candor now!");
            var process = new Process
            {
                StartInfo =
                {
                    // TODO: find java path or download it
                    FileName = "java",
                    Arguments = "-jar javaagent:/libs/candormanager.jar candormanager.jar",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    ErrorDialog = true
                }
            };
            process.OutputDataReceived += (sender, data) => {
                
                Log.ForContext<Process>().Information(data.Data);
            };
            process.StartInfo.RedirectStandardError = true;
            process.ErrorDataReceived += (sender, data) => {
                
                Log.ForContext<Process>().Error(data.Data);
            };
            if (process.Start())
            {
                Log.Debug("We have started candor!");
            }
            else
            {
                Log.Error("Unable to start candor.");
            }
            
            process.WaitForExitAsync();
            string output = process.StandardOutput.ReadToEnd();
            Log.Debug(output);
        }
    }
}