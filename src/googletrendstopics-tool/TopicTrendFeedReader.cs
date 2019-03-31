using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using googletrendstopics_tool;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Atom;
using Microsoft.SyndicationFeed.Rss;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ConsoleTableExt;

namespace googledtrendstopics_tool {
    public class TopicTrendFeedReader {
        string url = string.Empty;
        public TopicTrendFeedReader () {
            Geo = "US";
            Period = Period.Daily;
            url = $"https://trends.google.com/trends/trendingsearches/{Enum.GetName(Period.GetType(),Period).ToLower()}/rss?geo={Geo}";

        }

        public string FeedUrl { get; set; }

        [Option (ShortName = "g", Description = "Trends geo,Default value US")]

        public string Geo { get; set; }

        [Option (ShortName = "p", Description = "Trends period,Default value Daily")]
        public Period Period { get; set; }

        public async Task<int> OnExecute (CommandLineApplication app, IConsole console)
        {
            Console.WriteLine ($"{Enum.GetName(Period.GetType(),Period)} - {Geo} Google Topic Trends");
            var result = await CreateFeedResult();
            ConsoleTableBuilder.From(result)
                               .WithFormat(ConsoleTableBuilderFormat.Alternative)
                               .ExportAndWriteLine();
            return await Task.FromResult(Program.OK);

        }

        private async Task<List<FeedResult>> CreateFeedResult()
        {
            using (var xmlReader = XmlReader.Create(url, new XmlReaderSettings() { Async = true }))
            {
                List<FeedResult> feedResults = new List<FeedResult>();
                var reader = new RssFeedReader(xmlReader);

                while (await reader.Read())
                {
                    switch (reader.ElementType)
                    {

                        case SyndicationElementType.Item:
                            var item = await reader.ReadItem();
                            feedResults.Add(new FeedResult(){Title = item.Title,Description = item.Description});

                            //Console.WriteLine (item.Title);

                            break;
                    }
                }
                
                return await Task.FromResult(feedResults);
            }
        }
    }
    public class FeedResult {

        public string Title { get; set; }

        public string Description { get; set; }
    }
}