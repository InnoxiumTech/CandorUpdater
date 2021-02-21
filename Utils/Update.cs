using System.Collections.Generic;

namespace CandorUpdater.Utils
{
    public class UpdateStructure
    {
        public IList<Update> Updates { get; set; }
    }

    public class Update
    {
        public Version Version { get; set; }
        public string Url { get; set; }
    }
}