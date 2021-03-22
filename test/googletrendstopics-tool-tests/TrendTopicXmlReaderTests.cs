using Xunit;
using System.Threading.Tasks;
using System;
using System.IO;
using Shouldly;
using System.Collections;
using System.Collections.Generic;

namespace GoogleTrendsTopicsTool.Tests
{
    public class TrendTopicXmlReaderTests
    {

        [Fact]
        public async Task GetStreamAsync_ExceptedBehavior()
        {
           var trendTopicXmlReader = new TrendTopicXmlReader();
           var sampleRssPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"sample.rss"); 
           var sampleUri = new Uri(sampleRssPath);
           var stream= await trendTopicXmlReader.GetStreamAsync(sampleUri);
           stream.ShouldNotBeNull();
           stream.ShouldBeAssignableTo<Stream>();
        }
        [Fact]
        public async Task ReaderAsync_ExceptedBehavior()
        {
           var trendTopicXmlReader = new TrendTopicXmlReader();
           var sampleRssPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"sample.rss"); 
           var moqStream = File.OpenRead(sampleRssPath);
           var result = await trendTopicXmlReader.ReaderAsync(moqStream);
           result.ShouldBeAssignableTo<IList<FeedResult>>();
        }
    }
}