using System;
using System.Diagnostics.CodeAnalysis;
using GoogleTrendsTopicsTool;
using McMaster.Extensions.CommandLineUtils;

namespace googletrendstopics_tool
{
    [ExcludeFromCodeCoverage]
    class Program {
        public const int EXCEPTION = 2;
        public const int ERROR = 1;
        public const int OK = 0;
        static int Main(string[] args)
        {
            try
            {
                return CommandLineApplication.Execute<TopicTrendFeedReader>(args);
            }
            catch (System.Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Unexpected error: {ex}");
                Console.ResetColor();
                return EXCEPTION;
            }

        }
    }
}