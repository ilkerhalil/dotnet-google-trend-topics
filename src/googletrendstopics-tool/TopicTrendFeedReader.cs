using System;
using System.Threading.Tasks;
using googletrendstopics_tool;
using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using ConsoleTableExt;
using System.Xml.Linq;

namespace GoogleTrendsTopicsTool
{
    public class TopicTrendFeedReader
    {
        private const string baseUrl = "https://trends.google.com/trends/trendingsearches/daily";

        private IXmlReader _xmlReader;

        public TopicTrendFeedReader(IXmlReader xmlReader)
        {
            Geo = "US";
            _xmlReader = xmlReader;
            Url = $"{baseUrl}/rss?geo={Geo.ToString()}";
        }

        [Option(ShortName = "g", Description = "Trends geo,Default value US")]
        public string Geo { get; }

        [Option(ShortName = "u", Description = "The Url")]
        public string Url { get; }

        /// <inheritdoc />
        public async Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            console.WriteLine($"{Url} for Google Topic Trends");
            var stream = await _xmlReader.GetStreamAsync(new Uri(Url));
            List<FeedResult> result = await _xmlReader.ReaderAsync(stream) as List<FeedResult>;
            ConsoleTableBuilder.From(result)
                               .WithFormat(ConsoleTableBuilderFormat.Alternative)
                               .ExportAndWriteLine();
            return await Task.FromResult(Program.OK);
        }
    }
}