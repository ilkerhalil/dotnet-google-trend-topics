using System;
using System.Threading.Tasks;
using System.Xml;
using googletrendstopics_tool;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System.Collections.Generic;
using ConsoleTableExt;

namespace googledtrendstopics_tool {
    public class TopicTrendFeedReader : BaseTopicTrendFeedReader
    {
        private string _url = string.Empty;
        private const string baseUrl = "https://trends.google.com/trends/trendingsearches/daily";

        public TopicTrendFeedReader () {
            Geo = "US";
            
            

        }

        [Option (ShortName = "g", Description = "Trends geo,Default value US")]
        public  string Geo { get; set; }



        /// <inheritdoc />
        public override async Task<int> OnExecute (CommandLineApplication app, IConsole console)
        {
            Console.WriteLine ($"{Geo} Google Topic Trends");
            var result = await CreateFeedResult();
            ConsoleTableBuilder.From(result)
                
                               .WithFormat(ConsoleTableBuilderFormat.Alternative)
                               .ExportAndWriteLine();
            return await Task.FromResult(Program.OK);
        }

        protected override async Task<List<FeedResult>> CreateFeedResult()
        {
            _url = $"{baseUrl}/rss?geo={Geo}";
            using (var xmlReader = XmlReader.Create(_url, new XmlReaderSettings() { Async = true }))
            {
                var feedResults = new List<FeedResult>();
                var reader = new RssFeedReader(xmlReader);

                while (await reader.Read())
                {
                    switch (reader.ElementType)
                    {
                        case SyndicationElementType.Item:
                            var item = await reader.ReadItem();
                            feedResults.Add(new FeedResult(){Trend = item.Title,Description = item.Description});
                            break;
                        case SyndicationElementType.None:
                            break;
                        case SyndicationElementType.Person:
                            break;
                        case SyndicationElementType.Link:
                            break;
                        case SyndicationElementType.Content:
                            break;
                        case SyndicationElementType.Category:
                            break;
                        case SyndicationElementType.Image:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                
                return await Task.FromResult(feedResults);
            }
        }
    }
}