using CommandLine;

namespace CandorUpdater.Utils
{
    public class CliOptions
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
        
        [Option('u', "doupdate", Required = false, HelpText = "Sets whether we should do the update.", Default = false)]
        public bool DoUpdate { get; set; }

        [Option('l', "link", Required = false, HelpText = "Sets the link to read update data from.", Default = "https://innoxium.co.uk/api/updates")]
        public string UpdateLink { get; set; }
    }
}