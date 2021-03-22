using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;

namespace GoogleTrendsTopicsTool
{
    public class TrendTopicXmlReader : IXmlReader
    {
        public async Task<Stream> GetStreamAsync(Uri uri)
        {
            using (WebClient webClient = new WebClient())
            {
                return await Task.FromResult(webClient.OpenRead(uri));
            }
        }

        public async Task<IList<FeedResult>> ReaderAsync(Stream stream)
        {
            using (var xmlReader = XmlReader.Create(stream, new XmlReaderSettings() { Async = true }))
            {
                var feedResults = new List<FeedResult>();
                var reader = new RssFeedReader(xmlReader);
                while (await reader.Read())
                {
                    switch (reader.ElementType)
                    {
                        case SyndicationElementType.Item:
                            var item = await reader.ReadItem();
                            feedResults.Add(new FeedResult() { Trend = item.Title, Description = item.Description });
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
                    }
                }
                return await Task.FromResult(feedResults);
            }

        }


    }
}