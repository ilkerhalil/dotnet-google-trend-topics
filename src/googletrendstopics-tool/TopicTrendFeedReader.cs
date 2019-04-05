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
    public class TopicTrendFeedReader : BaseTopicTrendFeedReader
    {
        private readonly string _url = string.Empty;
        private const string baseUrl = "https://trends.google.com/trends/trendingsearches/";

        public TopicTrendFeedReader () {
            Geo = "US";
            Period = Period.Daily;
            _url = $"{baseUrl}{Enum.GetName(Period.GetType(),Period).ToLower()}/rss?geo={Geo}";

        }

        [Option (ShortName = "g", Description = "Trends geo,Default value US")]
        public string Geo { get; set; }

        [Option (ShortName = "p", Description = "Trends period,Default value Daily")]
        public Period Period { get; set; }

        /// <inheritdoc />
        public override async Task<int> OnExecute (CommandLineApplication app, IConsole console)
        {
            Console.WriteLine ($"{Enum.GetName(Period.GetType(),Period)} - {Geo} Google Topic Trends");
            var result = await CreateFeedResult();
            ConsoleTableBuilder.From(result)
                               .WithFormat(ConsoleTableBuilderFormat.Alternative)
                               .ExportAndWriteLine();
            return await Task.FromResult(Program.OK);
        }

        protected override async Task<List<FeedResult>> CreateFeedResult()
        {
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
                            feedResults.Add(new FeedResult(){Title = item.Title,Description = item.Description});
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