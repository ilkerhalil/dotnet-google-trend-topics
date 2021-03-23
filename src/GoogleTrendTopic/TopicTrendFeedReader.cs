using System;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using System.Collections.Generic;
using ConsoleTables;

namespace GoogleTrendsTopicsTool
{
    public class TopicTrendFeedReader
    {
        private const string baseUrl = "https://trends.google.com/trends/trendingsearches/daily";

        private IXmlReader _xmlReader;

        public TopicTrendFeedReader(IXmlReader xmlReader)
        {
            _xmlReader = xmlReader;
            
        }

        [Option(ShortName = "g", Description = "Trends geo,Default value US")]
        public string Geo { get;private set; } = "US";

        [Option(ShortName = "u", Description = "The Base Url")]
        public string BaseUrl { get;} = baseUrl;

        /// <inheritdoc />
        public async Task<int> OnExecute(CommandLineApplication app, IConsole console)
        {
            var url = $"{baseUrl}/rss?geo={Geo.ToString()}";
            console.WriteLine($"{url} for Google Topic Trends");
            var stream = await _xmlReader.GetStreamAsync(new Uri(url));
            List<FeedResult> result = await _xmlReader.ReaderAsync(stream) as List<FeedResult>;
            ConsoleTable
                .From<FeedResult>(result)
                .Configure(o => o.NumberAlignment = Alignment.Left)
                .Write(Format.Alternative);
            return await Task.FromResult(Program.OK);
        }
    }
}