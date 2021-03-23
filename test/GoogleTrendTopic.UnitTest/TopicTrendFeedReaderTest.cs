using System.Threading.Tasks;
using AutoBogus;
using Moq;
using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Collections.Generic;
using Xunit;

namespace GoogleTrendsTopicsTool.Tests
{
    public class TopicTrendFeedReaderTests
    {
        private readonly Moq.MockRepository _mockRepository;
        private readonly Mock<IXmlReader> _mockXmlReader;

        public TopicTrendFeedReaderTests()
        {
            _mockRepository = new Moq.MockRepository(MockBehavior.Strict);
            _mockXmlReader = _mockRepository.Create<IXmlReader>();
        }
        private TopicTrendFeedReader CreateTopicTrendFeedReader()
        {
            return new TopicTrendFeedReader(_mockXmlReader.Object);
        }
        [Fact]
        public async Task OnExecute_ExceptedBehavior()
        {
            var topicTrendFeedReader = CreateTopicTrendFeedReader();
            var moqCommandLineApplication = new CommandLineApplication();
            var moqConsole = _mockRepository.Create<McMaster.Extensions.CommandLineUtils.IConsole>();
            TextWriter textWriter = new StringWriter();
            IList<FeedResult> feedResults = AutoFaker.Generate<FeedResult>(5);
            _mockXmlReader
                .Setup(st => st.GetStreamAsync(It.IsAny<Uri>()))
                .ReturnsAsync(It.IsAny<Stream>());
            _mockXmlReader
                .Setup(st => st.ReaderAsync(It.IsAny<Stream>()))
                .ReturnsAsync(feedResults);
            moqConsole
            .Setup(st=> st.Out)
            .Returns(textWriter);

            await topicTrendFeedReader.OnExecute(moqCommandLineApplication, moqConsole.Object);
            _mockRepository.Verify();
        }
    }
}