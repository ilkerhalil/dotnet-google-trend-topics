using System.Collections.Generic;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace googledtrendstopics_tool
{
    public abstract  class BaseTopicTrendFeedReader
    {
        public string FeedUrl { get; set; }
        public string Geo { get; set; }
        public Period Period { get; set; }
        public abstract Task<int> OnExecute (CommandLineApplication app, IConsole console);
        protected abstract Task<List<FeedResult>> CreateFeedResult();
    }
}