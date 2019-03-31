using System;
using googledtrendstopics_tool;
using McMaster.Extensions.CommandLineUtils;
using System.Threading.Tasks;


namespace googletrendstopics_tool {
    class Program {
        public const int EXCEPTION = 2;
        public const int ERROR = 1;
        public const int OK = 0;
                
        static async Task<int> Main (string[] args) {
            try {
                return CommandLineApplication.Execute<TopicTrendFeedReader> (args);
            } catch (System.Exception ex) {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine ($"Unexpected error: {ex}");
                Console.ResetColor ();
                return EXCEPTION;
            }

        }
    }
}