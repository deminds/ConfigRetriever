using System;

namespace GH.DD.ConfigRetriever.Loggers
{
    public class StdoutLogger : ILogger
    {
        public void Debug(string message)
        {
            Console.WriteLine(message);
        }
    }
}