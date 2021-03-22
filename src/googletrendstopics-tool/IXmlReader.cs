using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoogleTrendsTopicsTool
{
    public interface IXmlReader
    {
        Task<Stream> GetStreamAsync(Uri uri);
        Task<IList<FeedResult>> ReaderAsync(Stream stream);
    }
}