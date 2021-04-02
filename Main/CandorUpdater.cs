using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CandorUpdater.Utils;
using CommandLine;
using Serilog;

namespace CandorUpdater.Main
{
    internal class CandorUpdater
    {
        public static CliOptions Opts;
        
        private const string LogFileName = "candorlauncher.log";
        private const string BakLogFileName = "candorlauncher.bak.log";

        private static async Task Main(string[] args)
        {
            if (File.Exists(LogFileName))
            {
                File.Copy(LogFileName, BakLogFileName, true);
                File.Delete(LogFileName);
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File(LogFileName)
                .CreateLogger();
            
            Log.Information("Logger has be configured.");
            
            var result = Parser.Default.ParseArguments<CliOptions>(args)
                .WithParsed(ParseArgs)
                .WithNotParsed(HandleParseError);
            
            // Lets try to update the program now
            // TODO: Add updating mechanism
            await CandorDownloader.DownloadCandor();
            
            // Now lets try to launch the program
            CandorStarter.StartCandor();
        }

        static void ParseArgs(CliOptions opts)
        {
            if(opts.Verbose)
            {
                Log.Debug("We are verbose");
            }

            Opts = opts;
        }

        static void HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                if (error.StopsProcessing)
                {
                    Environment.Exit(ExitCodes.ParseError);
                }
            }
        }
    }
}