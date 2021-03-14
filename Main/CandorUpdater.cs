using System;
using System.Collections.Generic;
using CandorUpdater.Utils;
using CommandLine;
using Serilog;
using Serilog.Core;

namespace CandorUpdater.Main
{
    internal class CandorUpdater
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("candorlauncher.log")
                .CreateLogger();
            
            Log.Information("Logger has be configured.");
            
            var result = Parser.Default.ParseArguments<CliOptions>(args)
                .WithParsed(ParseArgs)
                .WithNotParsed(HandleParseError);
            
            // Lets try to update the program now
            // TODO: Add updating mechanism
            
            // Now lets try to launch the program
            CandorStarter.StartCandor();
        }

        static void ParseArgs(CliOptions opts)
        {
            if(opts.Verbose)
            {
                Log.Debug("We are verbose");
            }
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