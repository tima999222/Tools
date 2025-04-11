using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Events;
using Serilog.Debugging;

namespace Tools.Logging
{
    public static class LoggingConfigurator
    {
        public static ILogger ElasticLogger(string indexFormat, string httpUser, string httpPassword,
            string url = "http://localhost:9200", string logLevel = "warning")
        {
            if (indexFormat == null) throw new ArgumentNullException(nameof(indexFormat));
            if (httpUser == null) throw new ArgumentNullException(nameof(httpUser));
            if (httpPassword == null) throw new ArgumentNullException(nameof(httpPassword));
            if (url == null) throw new ArgumentNullException(nameof(url));

            var cfg = ElasticConfiguration(indexFormat,
                httpUser, httpPassword, url, logLevel);

            var logger = cfg.CreateLogger();

            return logger;
        }

        public static ILogger ConsoleLogger(string logLevel = "warning")
        {
            return new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Level:u3} {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Is(GetMinimumLogLevel(logLevel))
                .CreateLogger();
        }

        public static LoggerConfiguration ElasticConfiguration(string indexFormat, string apiUser, string apiKey,
            string url = "http://localhost:9200", string logLevel = "warning")
        {
            SelfLog.Enable(Console.WriteLine);

            var elasticsearchSinkOptions = new ElasticsearchSinkOptions(new Uri(url))
            {
                ModifyConnectionSettings =
                    x => x.BasicAuthentication(apiUser, apiKey).EnableApiVersioningHeader(),

                AutoRegisterTemplate = false,

                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                IndexFormat = $"{DateTime.UtcNow.ToyyyyMMdd()}-{indexFormat}",
                BatchAction = ElasticOpType.Index,
                OverwriteTemplate = true,
                TypeName = null,
                FailureCallback = FailureCallback,
                EmitEventFailure = EmitEventFailureHandling.ThrowException,
            };

            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithCorrelationId()
                .Enrich.WithExceptionData()
                .Enrich.WithMachineName()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .WriteTo.Elasticsearch(elasticsearchSinkOptions)
                .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .MinimumLevel.Is(GetMinimumLogLevel(logLevel));
        }

        private static void FailureCallback(LogEvent e, Exception exception)
        {
            Console.WriteLine("Unable to submit event " + e.MessageTemplate);
            throw e.Exception;
        }

        private static LogEventLevel GetMinimumLogLevel(string logLevel)
        {
            return logLevel.ToLower() switch
            {
                "verbose" => LogEventLevel.Verbose,
                "debug" => LogEventLevel.Debug,
                "information" => LogEventLevel.Information,
                "warning" => LogEventLevel.Warning,
                "error" => LogEventLevel.Error,
                "fatal" => LogEventLevel.Fatal,
                _ => LogEventLevel.Information
            };
        }
    }

}
