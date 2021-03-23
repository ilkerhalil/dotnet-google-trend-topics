using System;
using System.Diagnostics.CodeAnalysis;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleTrendsTopicsTool
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
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddTransient<IXmlReader,TrendTopicXmlReader>();
                var serviceProvider = serviceCollection.BuildServiceProvider();
                var commandLineApplication = new CommandLineApplication<TopicTrendFeedReader>();
                commandLineApplication.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(serviceProvider);
                return commandLineApplication.Execute(args);
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