namespace ApiCatalogo.Logging
{
    public class CustomerLogger : ILogger
    {
        private readonly string loggerName;
        private readonly CustomLoggerProviderConfiguration loggerConfig;

        public CustomerLogger(string categoryName, CustomLoggerProviderConfiguration config)
        {
            loggerName = categoryName;
            loggerConfig = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = $"{logLevel.ToString()} - {eventId.Id} - {formatter(state, exception)}";

            WriteTextToFile(message);
        }

        private void WriteTextToFile(string message)
        {
            string filePath = @"C:\Users\renan\Projects\net8-api-macoratti\section3-5\ApiCatalogo\CustomLog.txt";

            using (StreamWriter streamWriter = new StreamWriter(filePath, true))
            {
                try
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
