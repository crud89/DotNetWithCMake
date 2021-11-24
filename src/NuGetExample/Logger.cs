using System;

namespace Example
{
    using Serilog;

    public static class Logger
    {
        private static ILogger Console = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        public static void Log(string message) 
        {
            Console.Information(message);
        }
    }
}