using System;
using System.Collections.Generic;
using CandorUpdater.Utils;
using CommandLine;

namespace CandorUpdater.Main
{
    class CandorUpdater
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<CliOptions>(args)
                .WithParsed(ParseArgs)
                .WithNotParsed(HandleParseError);
            
        }

        static void ParseArgs(CliOptions opts)
        {
            if(opts.Verbose)
            {
                Console.WriteLine("We are verbose");
            }
        }

        static void HandleParseError(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                // Console.WriteLine(error);
            }
        }
    }
}